using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Media.Media3D;

namespace ChessCompanion.MVVM.Utility
{
    public class Engine : IEngine
    {
        private Process _process;

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
        public string AnalyzeLastMove(TopMove lastBestMove, TopMove currentMove)
        {
            string lastMoveScore;
            if (lastBestMove.FEN == currentMove.FEN)
            {
                lastMoveScore = "BestMove";
            }
            else
            {
                if (lastBestMove.mate != null)
                {
                    // if last move is losing mate, this move just escapes a mate
                    // if last move is winning mate, this move is a missed win
                    if (currentMove.mate == null)
                    {
                        lastMoveScore = lastBestMove.mate > 0 ? "MissedWin" : "Brilliant";
                    }
                    else
                    {
                        //both are mate
                        lastMoveScore = lastBestMove.mate > 0 ? "Excellent" : "ResignWhite";
                    }
                }
                else if (currentMove.mate != null)
                {
                    // brilliant if it found a mate, blunder if it moved into a mate
                    lastMoveScore = currentMove.mate < 0 ? "Brilliant" : "Blunder";
                }
                else if (currentMove.cp != null && lastBestMove.cp != null) 
                {
                    int? evalDiff = -(currentMove.cp + lastBestMove.cp);
                    if (evalDiff > 100)
                        lastMoveScore = "Brilliant";
                    else if (evalDiff > 0)
                        lastMoveScore = "GreatFind";
                    else if (evalDiff > -10)
                        lastMoveScore = "BestMove";
                    else if (evalDiff > -25)
                        lastMoveScore = "Excellent";
                    else if (evalDiff > -50)
                        lastMoveScore = "Good";
                    else if (evalDiff > -100)
                        lastMoveScore = "Inaccuracy";
                    else if (evalDiff > -250)
                        lastMoveScore = "Mistake";
                    else
                        lastMoveScore = "Blunder";
                }
                else
                {
                    throw new ArgumentException("Analyzing last move error");
                }
            }
            Debug.WriteLine("Value: " + lastMoveScore);
            return lastMoveScore;
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
