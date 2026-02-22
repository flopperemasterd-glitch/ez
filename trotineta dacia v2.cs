using System;
using System.Text;
using System.Threading;
using System.Collections.Generic;

namespace kaki
{
    class Program
    {
        static bool spinEnabled = false;
        static float A = 0, B = 0, C = 0;

        const int width = 120;
        const int height = 35;
        static float[] zBuffer = new float[width * height];
        static char[] buffer = new char[width * height];
        const char backgroundASCIICode = ' ';
        const int distanceFromCam = 60;
        const float K1 = 20;
        const float incrementSpeed = 0.8f;

        static char frontChar = '@';
        static char rightChar = '#';
        static char leftChar = '~';
        static char backChar = '$';
        static char bottomChar = '+';
        static char topChar = '*';

        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            try
            {
                Console.SetBufferSize(width + 1, height + 25);
            }
            catch { }

            Console.WriteLine("╔════════════════════════════════════╗");
            Console.WriteLine("║     CUBE FACE CHARACTER EDITOR     ║");
            Console.WriteLine("╚════════════════════════════════════╝");
            Console.WriteLine("\nCustomize characters for each cube face:");
            Console.WriteLine("(Press Enter to skip and use defaults)");

            Console.Write($"\nFront face [{frontChar}]: ");
            string input = Console.ReadLine();
            if (!string.IsNullOrEmpty(input)) frontChar = input[0];

            Console.Write($"Right face [{rightChar}]: ");
            input = Console.ReadLine();
            if (!string.IsNullOrEmpty(input)) rightChar = input[0];

            Console.Write($"Left face [{leftChar}]: ");
            input = Console.ReadLine();
            if (!string.IsNullOrEmpty(input)) leftChar = input[0];

            Console.Write($"Back face [{backChar}]: ");
            input = Console.ReadLine();
            if (!string.IsNullOrEmpty(input)) backChar = input[0];

            Console.Write($"Bottom face [{bottomChar}]: ");
            input = Console.ReadLine();
            if (!string.IsNullOrEmpty(input)) bottomChar = input[0];

            Console.Write($"Top face [{topChar}]: ");
            input = Console.ReadLine();
            if (!string.IsNullOrEmpty(input)) topChar = input[0];

            Console.WriteLine("\n✓ Face characters configured!");
            Thread.Sleep(500);

            Console.WriteLine("\nEnable cube spinning? (y/n)");
            var key = Console.ReadKey(true);
            spinEnabled = (key.KeyChar == 'y' || key.KeyChar == 'Y');

            // organisation at its finest
            var questions = new List<(string question, string[] answers, int correct, string[] hints)>
            {
                // -- BLACK HOLE --
                ("What is a black hole?",
                    new[] {"1", "2", "3"}, 2,
                    new[] {
                        "1: a region of space with no matter",
                        "2: a very dense star that still emits light",
                        "3: a singularity with gravity so strong light cant escape"
                    }),

                ("What is the event horizon of a black hole?",
                    new[] {"1", "2", "3"}, 2,
                    new[] {
                        "1: the glowing accretion disk around it",
                        "2: the singularity at the center",
                        "3: the boundary where escape velocity exceeds light speed"
                    }),

                ("How are black holes detected if they emit no light?",
                    new[] {"1", "2", "3"}, 0,
                    new[] {
                        "1: by observing gravitational effects on nearby matter",
                        "2: by the colour they emit in infrared",
                        "3: by the sound waves they produce"
                    }),

                ("What is Hawking Radiation?",
                    new[] {"1", "2", "3"}, 1,
                    new[] {
                        "1: radiation from the accretion disk of a BH",
                        "2: theoretical thermal radiation slowly emitted by a BH",
                        "3: the light visible just outside the event horizon"
                    }),

                ("Inside a black hole, r (radius) becomes what?",
                    new[] {"1", "2", "3"}, 2,
                    new[] {
                        "1: a spatial dimension like normal",
                        "2: irrelevant and equal to zero",
                        "3: a time-like coordinate — everything moves toward r=0"
                    }),

                // -- WHITE HOLE --
                ("What is a white hole?",
                    new[] {"1", "2", "3"}, 0,
                    new[] {
                        "1: the time-reverse of a black hole — matter can only exit",
                        "2: a black hole that has turned cold",
                        "3: a hole in space with white light emitting from it"
                    }),

                ("What equation predicts white holes exist?",
                    new[] {"1", "2", "3"}, 1,
                    new[] {
                        "1: Maxwells equations of electromagnetism",
                        "2: Einsteins field equations of general relativity",
                        "3: Schrodingers wave equation"
                    }),

                ("Have white holes ever been observed?",
                    new[] {"1", "2", "3"}, 2,
                    new[] {
                        "1: yes, near the center of the Milky Way",
                        "2: yes, indirectly through gravitational lensing",
                        "3: no, they remain purely theoretical so far"
                    }),

                ("What is a Penrose diagram used for?",
                    new[] {"1", "2", "3"}, 0,
                    new[] {
                        "1: mapping the full spacetime of a black hole and white hole",
                        "2: calculating the mass of a black hole",
                        "3: predicting Hawking radiation temperature"
                    }),

                // -- GREY HOLE --
                ("What did Hawking propose in 2014 about black holes?",
                    new[] {"1", "2", "3"}, 1,
                    new[] {
                        "1: that black holes dont actually exist",
                        "2: replace the event horizon with an apparent horizon",
                        "3: that black holes emit visible light after all"
                    }),

                ("What is a grey hole (apparent horizon concept)?",
                    new[] {"1", "2", "3"}, 0,
                    new[] {
                        "1: a BH where matter/energy can eventually escape via quantum effects",
                        "2: a black hole that has cooled down completely",
                        "3: a wormhole connecting two black holes"
                    }),

                ("What is the firewall paradox related to?",
                    new[] {"1", "2", "3"}, 2,
                    new[] {
                        "1: the intense heat of the accretion disk",
                        "2: nuclear fusion near the singularity",
                        "3: a conflict between quantum mechanics and general relativity at the horizon"
                    }),

                // -- WORMHOLE --
                ("What is a wormhole?",
                    new[] {"1", "2", "3"}, 0,
                    new[] {
                        "1: a hypothetical tunnel connecting two points in spacetime",
                        "2: a type of black hole with a rotating singularity",
                        "3: a region of space where time stops"
                    }),

                ("What is another name for a wormhole?",
                    new[] {"1", "2", "3"}, 1,
                    new[] {
                        "1: Kerr bridge",
                        "2: Einstein-Rosen bridge",
                        "3: Schwarzschild tunnel"
                    }),

                ("What is needed to keep a wormhole stable and open?",
                    new[] {"1", "2", "3"}, 2,
                    new[] {
                        "1: a rotating black hole at each end",
                        "2: extremely high temperatures",
                        "3: exotic matter with negative energy density"
                    }),

                ("What is spacetime curvature caused by?",
                    new[] {"1", "2", "3"}, 0,
                    new[] {
                        "1: mass and energy bending the fabric of spacetime",
                        "2: the rotation speed of a planet",
                        "3: electromagnetic fields around large objects"
                    }),           
            };

            int score = 0;

            for (int q = 0; q < questions.Count; q++)
            {
                var (question, answers, correct, hints) = questions[q];
                bool answered = false;
                int selected = -1;
                DateTime startTime = DateTime.Now;
                double timeLimit = 15.0; // slightly more time since questions are harder

                if (!spinEnabled)
                {
                    A = 0.8f;
                    B = 0.5f;
                    C = 0.0f;
                }

                StringBuilder sb = new StringBuilder(width * (height + 20));

                while (!answered)
                {
                    double elapsed = (DateTime.Now - startTime).TotalSeconds;
                    double remaining = Math.Max(0, timeLimit - elapsed);

                    if (remaining <= 0)
                    {
                        break;
                    }

                    Array.Fill(buffer, backgroundASCIICode);
                    Array.Fill(zBuffer, 0f);

                    RenderCube(8, -30, answers[0], 10);
                    RenderCube(8, 0, answers[1], 10);
                    RenderCube(8, 30, answers[2], 10);

                    sb.Clear();

                    // Question header
                    sb.AppendLine($"╔═══ QUESTION {q + 1}/{questions.Count} ═══════════════════════════════════════════════════╗");
                    sb.AppendLine($"║  {question.PadRight(width - 4)}║");
                    sb.AppendLine("╚" + new string('═', width - 2) + "╝");
                    sb.AppendLine();

                    // 3D cubes
                    for (int k = 0; k < height * width; k++)
                    {
                        if (k % width == 0 && k > 0)
                            sb.Append('\n');
                        sb.Append(buffer[k]);
                    }

                    sb.AppendLine();
                    sb.AppendLine();

                    int barWidth = 40;
                    int filled = (int)((remaining / timeLimit) * barWidth);
                    sb.Append("TIME: [");
                    sb.Append(new string('█', filled));
                    sb.Append(new string('░', barWidth - filled));
                    sb.AppendLine($"] {remaining:F1}s");

                    sb.AppendLine();

                    sb.AppendLine("╔═══ HINTS ══════════════════════════════════════════════════════════════════════╗");
                    sb.AppendLine($"║  [A] {hints[0].PadRight(width - 7)}║");
                    sb.AppendLine($"║  [S] {hints[1].PadRight(width - 7)}║");
                    sb.AppendLine($"║  [D] {hints[2].PadRight(width - 7)}║");
                    sb.AppendLine("╚" + new string('═', width - 2) + "╝");

                    sb.AppendLine("\n       [A] Left          [S] Middle          [D] Right");

                    Console.SetCursorPosition(0, 0);
                    Console.Write(sb.ToString());

                    if (Console.KeyAvailable)
                    {
                        var input2 = Console.ReadKey(true);
                        if (input2.Key == ConsoleKey.A) { selected = 0; answered = true; }
                        else if (input2.Key == ConsoleKey.S) { selected = 1; answered = true; }
                        else if (input2.Key == ConsoleKey.D) { selected = 2; answered = true; }
                    }

                    if (spinEnabled)
                    {
                        A += 0.04f;
                        B += 0.04f;
                        C += 0.01f;
                    }

                    Thread.Sleep(50);
                }

                Console.Clear();
                if (selected == correct)
                {
                    score++;
                    Console.WriteLine("\n\n╔════════════════════════════════════╗");
                    Console.WriteLine("║            CORRECT!                ║");
                    Console.WriteLine("╚════════════════════════════════════╝");
                }
                else
                {
                    string correctHint = hints[correct];
                    Console.WriteLine("\n\n╔════════════════════════════════════════════════════════════╗");
                    Console.WriteLine("║            WRONG!                                          ║");
                    Console.WriteLine($"║  Correct answer: {answers[correct].PadRight(40)}║");
                    Console.WriteLine($"║  {correctHint.PadRight(58)}║");
                    Console.WriteLine("╚════════════════════════════════════════════════════════════╝");
                }
                Thread.Sleep(2000);
            }

            Console.Clear();
            Console.WriteLine("\n\n╔════════════════════════════════════╗");
            Console.WriteLine("║      SIMULATION COMPLETE           ║");
            Console.WriteLine("╚════════════════════════════════════╝");
            Console.WriteLine($"\nFinal Score: {score}/{questions.Count}");
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        static void RenderCube(float cubeWidth, float horizontalOffset, string answer, int yOffset)
        {
            for (float cubeX = -cubeWidth; cubeX < cubeWidth; cubeX += incrementSpeed)
            {
                for (float cubeY = -cubeWidth; cubeY < cubeWidth; cubeY += incrementSpeed)
                {
                    CalculateForSurface(cubeX, cubeY, -cubeWidth, frontChar, horizontalOffset, yOffset);
                    CalculateForSurface(cubeWidth, cubeY, cubeX, rightChar, horizontalOffset, yOffset);
                    CalculateForSurface(-cubeWidth, cubeY, -cubeX, leftChar, horizontalOffset, yOffset);
                    CalculateForSurface(-cubeX, cubeY, cubeWidth, backChar, horizontalOffset, yOffset);
                    CalculateForSurface(cubeX, -cubeWidth, -cubeY, bottomChar, horizontalOffset, yOffset);
                    CalculateForSurface(cubeX, cubeWidth, cubeY, topChar, horizontalOffset, yOffset);
                }
            }

            float textZ = -cubeWidth;
            float charSpacing = 2.8f;
            int numChars = answer.Length;
            float totalWidth = (numChars - 1) * charSpacing;
            float startLocalX = -totalWidth / 2f;

            for (int i = 0; i < numChars; i++)
            {
                float localX = startLocalX + i * charSpacing;
                float localY = 0f;

                float x = CalculateX(localX, localY, textZ);
                float y = CalculateY(localX, localY, textZ);
                float z = CalculateZ(localX, localY, textZ) + distanceFromCam;

                if (z <= 0) continue;

                float ooz = 1 / z;
                ooz += 0.001f;

                int xp = (int)(width / 2 + horizontalOffset + K1 * ooz * x * 2);
                int yp = (int)(height / 2 + yOffset + K1 * ooz * y);

                if (xp >= 0 && xp < width && yp >= 0 && yp < height)
                {
                    int idx = xp + yp * width;
                    if (ooz >= zBuffer[idx])
                    {
                        zBuffer[idx] = ooz;
                        buffer[idx] = answer[i];
                    }
                }
            }
        }

        static void CalculateForSurface(float cubeX, float cubeY, float cubeZ, char ch, float horizontalOffset, int yOffset)
        {
            float x = CalculateX(cubeX, cubeY, cubeZ);
            float y = CalculateY(cubeX, cubeY, cubeZ);
            float z = CalculateZ(cubeX, cubeY, cubeZ) + distanceFromCam;

            if (z <= 0) return;

            float ooz = 1 / z;

            int xp = (int)(width / 2 + horizontalOffset + K1 * ooz * x * 2);
            int yp = (int)(height / 2 + yOffset + K1 * ooz * y);

            if (xp >= 0 && xp < width && yp >= 0 && yp < height)
            {
                int idx = xp + yp * width;
                if (ooz > zBuffer[idx])
                {
                    zBuffer[idx] = ooz;
                    buffer[idx] = ch;
                }
            }
        }

        static float CalculateX(float i, float j, float k)
        {
            return j * (float)Math.Sin(A) * (float)Math.Sin(B) * (float)Math.Cos(C) -
                   k * (float)Math.Cos(A) * (float)Math.Sin(B) * (float)Math.Cos(C) +
                   j * (float)Math.Cos(A) * (float)Math.Sin(C) +
                   k * (float)Math.Sin(A) * (float)Math.Sin(C) +
                   i * (float)Math.Cos(B) * (float)Math.Cos(C);
        }

        static float CalculateY(float i, float j, float k)
        {
            return j * (float)Math.Cos(A) * (float)Math.Cos(C) +
                   k * (float)Math.Sin(A) * (float)Math.Cos(C) -
                   j * (float)Math.Sin(A) * (float)Math.Sin(B) * (float)Math.Sin(C) +
                   k * (float)Math.Cos(A) * (float)Math.Sin(B) * (float)Math.Sin(C) -
                   i * (float)Math.Cos(B) * (float)Math.Sin(C);
        }

        static float CalculateZ(float i, float j, float k)
        {
            return k * (float)Math.Cos(A) * (float)Math.Cos(B) -
                   j * (float)Math.Sin(A) * (float)Math.Cos(B) +
                   i * (float)Math.Sin(B);
        }
    }
}
