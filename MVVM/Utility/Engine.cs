using System;
using System.Diagnostics;
using System.Linq;

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

        public (string bestMove, int cp, string pv) GetBestMoveWithInfo(int searchTimeMs)
        {
            SendCommand($"go movetime {searchTimeMs}");

            string output = "";
            string bestMove = "";
            int cp = 0;
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
                    break;
                }
                else if (output.StartsWith("info"))
                {
                    string[] fields = output.Split(' ');
                    for (int i = 0; i < fields.Length - 1; i++)
                    {
                        if (fields[i] == "cp")
                        {
                            cp = int.Parse(fields[i + 1]);
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

            return (bestMove, cp, pv);
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
