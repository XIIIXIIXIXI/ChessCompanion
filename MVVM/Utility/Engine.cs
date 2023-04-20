using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Media.Media3D;

namespace ChessCompanion.MVVM.Utility
{
    public class Engine : IEngine
    {
        private Process _process;
        private int lines = 1;

        public Engine(string executablePath)
        {
            _process = new Process();
            _process.StartInfo.FileName = executablePath;
            _process.StartInfo.UseShellExecute = false;
            _process.StartInfo.RedirectStandardInput = true;
            _process.StartInfo.RedirectStandardOutput = true;
            _process.Start();
            Initialize();
        }
        private void Initialize()
        {
            SendCommand("uci");
            WaitForResponse("uciok");
        }
        public void SetPosition(string fen, params string[] moves)
        {
            string command = "position fen " + fen;
            if (moves.Length > 0)
            {
                command += " moves " + string.Join(" ", moves);
            }
            SendCommand(command);
        }

        public string GetBestMove(int searchTimeMs)
        {
            SendCommand($"go movetime {searchTimeMs}");
            return WaitForResponse("bestmove").Split(' ')[1];
        }

        public (string bestMove, int? cp, int? mate, bool promotion, string pv) GetBestMoveWithInfo(int searchTimeMs) 
        {
           
            SendCommand($"go movetime {searchTimeMs}");

            string output = "";
            string bestMove = "";
            int? cp = null;
            int? mate = null;
            bool promotion = false;
            string pv = "";

            while (true)
            {
                output = _process.StandardOutput.ReadLine();
                if (output == null)
                {
                    break;
                }
                else if (output.StartsWith("bestmove"))
                {
                    bestMove = output.Split(' ')[1];
                    if (bestMove.Length > 4)
                    {
                        promotion = true;
                    }
                    break;
                }
                else if (output.StartsWith("info"))
                {
                    string[] fields = output.Split(' ');
                    for (int i = 0; i < fields.Length - 1; i++)
                    {
                        if (fields[i] == "score")
                        {
                            if (fields[i + 1] == "mate")
                            {
                                // This is a mate in X moves
                                mate = int.Parse(fields[i + 2]);
                                /*if (fields[i + 2].StartsWith("-"))
                                {
                                    cp = -cp; // Black to mate
                                }*/
                                //cp = 10000 + cp;  Add 10000 to distinguish from regular centipawn values
                            }

                            else if (fields[i+1] == "cp")
                            {
                                // This is a regular centipawn value
                                cp = int.Parse(fields[i + 2]);
                            }
                        }
                        else if (fields[i] == "pv")
                        {
                            pv = "";
                            for (int j = i + 1; j < fields.Length && fields[j] != "bmc"; j++)
                            {
                                pv += fields[j] + " ";
                            }
                            pv = pv.Trim();
                        }
                    }
                }
            }
            
            return (bestMove, cp, mate, promotion, pv);
        }

        public void setLines(int number)
        {
            SendCommand($"setoption name MultiPV value {number}");
            this.lines = number;
        }
        public TopMove[] GetMultipleLines(int searchTimeMs)
        {
            SendCommand($"go movetime {searchTimeMs}");

            string output = "";
            string[] lastFiveLines = new string[lines+1]; //+1 since bestmove is also there
            int lineCount = 0;

            string bestMove = "";
            int? cp = null;
            int? mate = null;
            bool promotion = false;
            string pv = "";
            int bestMoveIndex = -1;
            TopMove[] topMoves = new TopMove[lines];

            while (true)
            {
                string currentLine = _process.StandardOutput.ReadLine();
                if (currentLine == null)
                {
                    break;
                }

                // Add the current line to the array of last five lines
                lastFiveLines[lineCount % (lines+1)] = currentLine;
                

                if (currentLine.StartsWith("bestmove"))
                {
                    // If the current line contains "bestmove", extract the best move and exit the loop
                    bestMove = currentLine.Split(' ')[1];
                    if (bestMove.Length > 4)
                    {
                        promotion = true;
                    }
                    // Store the index of the line containing the "bestmove" string
                    bestMoveIndex = lineCount % (lines+1);

                    if (bestMoveIndex != lines)
                    {
                        // Swap the element containing the bestmove string with the last element of the array
                        string temp = lastFiveLines[lines];
                        lastFiveLines[lines] = lastFiveLines[bestMoveIndex];
                        lastFiveLines[bestMoveIndex] = temp;
                    }
                    break;
                }
                lineCount++;
            }

            
            // Parse the last five lines for additional information
            for (int i = 0; i < lines; i++)
            {
                topMoves[i] = new TopMove();
                string currentLine = lastFiveLines[i];
                if (currentLine == null || !currentLine.StartsWith("info"))
                {
                    continue;
                }

                string[] fields = currentLine.Split(' ');
                for (int j = 0; j < fields.Length - 1; j++)
                {
                    if (fields[j] == "score")
                    {
                        if (fields[j + 1] == "mate")
                        {
                            // This is a mate in X moves
                            mate = int.Parse(fields[j + 2]);
                            /*if (fields[j + 2].StartsWith("-"))
                            {
                                cp = -cp; // Black to mate
                            }*/
                            //cp = 10000 + cp;  Add 10000 to distinguish from regular centipawn values
                        }

                        else if (fields[j + 1] == "cp")
                        {
                            // This is a regular centipawn value
                            cp = int.Parse(fields[j + 2]);
                        }
                    }
                    else if (fields[j] == "pv")
                    {
                        pv = "";
                        for (int k = j + 1; k < fields.Length && fields[k] != "bmc"; k++)
                        {
                            pv += fields[k] + " ";
                        }
                        pv = pv.Trim();
                    }
                    
                }
                
                topMoves[i].setTopMove(bestMove, cp, mate, promotion, pv);
            }
            if (topMoves.Length > 1)
            {
                OrderTopMoves(topMoves);
            }

            return topMoves;
        }

        public void OrderTopMoves(TopMove[] topMoves)
        {
            Array.Sort(topMoves, (move1, move2) =>
            {
                // If one move has a mate and the other doesn't, the mate wins
                if (move1.mate != null && move2.mate == null)
                {
                    return -1;
                }
                else if (move1.mate == null && move2.mate != null)
                {
                    return 1;
                }
                // If both moves have a mate, the one with the lower mate score wins
                else if (move1.mate != null && move2.mate != null)
                {
                    return move1.mate.Value.CompareTo(move2.mate.Value);
                }
                // If neither move has a mate, compare centipawn scores
                else
                {
                    return move2.cp.Value.CompareTo(move1.cp.Value);
                }
            });
        }

        private void SendCommand(string command)
        {
            _process.StandardInput.WriteLine(command);
        }

        private string WaitForResponse(string expectedOutput)
        {
            string output = "";
            while (!output.Contains(expectedOutput))
            {
                output = _process.StandardOutput.ReadLine();
            }
            return output;
        }
        public void SetOption(string name, object value)
        {
            string command = $"setoption name {name} value {value}";
            SendCommand(command);
        }

        public MoveScore AnalyzeLastMove(TopMove lastBestMove, TopMove currentMove)
        {
            if (lastBestMove.FEN == currentMove.FEN)
            {
                return MoveScore.BestMove;
            }
            else if (lastBestMove.mate != null)
            {
                if (currentMove.mate == null)
                {
                    return lastBestMove.mate > 0 ? MoveScore.MissedWin : MoveScore.Brilliant;
                }
                else
                {
                    return lastBestMove.mate > 0 ? MoveScore.Excellent : MoveScore.ResignWhite;
                }
            }
            else if (currentMove.mate != null)
            {
                return currentMove.mate < 0 ? MoveScore.Brilliant : MoveScore.Blunder;
            }
            else if (currentMove.cp != null && lastBestMove.cp != null)
            {
                int? evalDiff = -(currentMove.cp + lastBestMove.cp);
                if (evalDiff > 100)
                {
                    return MoveScore.Brilliant;
                }
                else if (evalDiff > 0)
                {
                    return MoveScore.GreatFind;
                }
                else if (evalDiff > -10)
                {
                    return MoveScore.BestMove;
                }
                else if (evalDiff > -25)
                {
                    return MoveScore.Excellent;
                }
                else if (evalDiff > -50)
                {
                    return MoveScore.Good;
                }
                else if (evalDiff > -120)
                {
                    return MoveScore.Inaccuracy;
                }
                else if (evalDiff > -250)
                {
                    return MoveScore.Mistake;
                }
                else
                {
                    return MoveScore.Blunder;
                }
            }
            else
            {
                throw new ArgumentException("Analyzing last move error");
            }
        }

        public void Dispose()
        {
            if (!_process.HasExited)
            {
                SendCommand("quit");
                _process.WaitForExit();
            }
            _process.Dispose();
        }
    }
}

