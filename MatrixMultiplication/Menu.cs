using System.Diagnostics;

namespace MatrixMultiplication {

    internal sealed class Menu {

        private const string PROJECT_TITLE = "Matrix Multiplication";

        // ##### Const variables [CHANGE THESE] #####

        // Controls the number of "threads" on parallelized methods
        private const int THREADS = 8;

        // Controls matrices bounds
        private const int M1_ROWS = 2000;
        private const int M1_COLS = 2000;
        private const int M2_ROWS = 2000;
        private const int M2_COLS = 2000;

        // Controls if matrices should be filled with a specific number
        // NULL = Incremented values (0,1,2,...) / NOT NULL = Specified number (across the whole matrix)
        private readonly float? M1_FILL = null;
        private readonly float? M2_FILL = null;

        // ##########################################

        // Matrices
        private readonly Matrix m1;
        private readonly Matrix m2;
        private readonly Matrix m2T;

        private readonly Matrix2D m1_2D;
        private readonly Matrix2D m2_2D;

        // Menu options
        private readonly string[] multMethods;

        public Menu(int width, int height) {

            // These methods stop the Console from flickering when clearing
            // but are only supported on Windows OS
            if (OperatingSystem.IsWindows()) {
                Console.SetWindowSize(width, height);
                Console.SetBufferSize(width, height);
            }

            // Hide the console's cursor
            Console.CursorVisible = false;

            // Set a project title
            Console.Title = PROJECT_TITLE;

            // The main menu's options
            multMethods = new string[6] {
                "Double Index [Single Thread]",
                "Double Index [Multithread]",
                "Linear Classic [Single Thread]",
                "Linear Classic [Multithread]",
                "Linear Transposed [Single Thread]",
                "Linear Transposed [Multithread]",
            };

            // Build the 1D matrices
            m1 = new(M1_ROWS, M1_COLS, true, M1_FILL);
            m2 = new(M2_ROWS, M2_COLS, true, M2_FILL);
            m2T = m2.Transposed;

            // Build the 2D matrices
            m1_2D = new(M1_ROWS, M1_COLS, true, M1_FILL);
            m2_2D = new(M2_ROWS, M2_COLS, true, M2_FILL);
        }

        public void DisplayMultMethods(int cursor = 0, int menu = 0, int first = -1, int second = -1) {

            // Clean the console
            Console.Clear();

            // Get the number of options
            int nOptions = multMethods.Length;

            if (menu == 0)
                Console.WriteLine("SELECT THE FIRST MULTIPLICATION METHOD\n");
            else
                Console.WriteLine("SELECT THE SECOND MULTIPLICATION METHOD\n");

            // The actual cursor (top) coordinates
            int[] cursorCoord = new int[nOptions + 1];

            // Print every option (+ its index)
            for (int i = 0; i < nOptions; i++) {

                cursorCoord[i] = Console.GetCursorPosition().Top;
                Console.WriteLine($"  {i + 1}." + multMethods[i] + (i % 2 != 0 ? '\n' : ""));
            }

            cursorCoord[nOptions] = cursorCoord[nOptions - 1] + 2;

            // Write the last option (of the current menu)
            Console.WriteLine($"  {nOptions + 1}." + (menu == 0 ? "Exit" : "Back"));

            // Print the 'cursor'
            Console.SetCursorPosition(0, cursorCoord[cursor]);
            Console.Write('►');

            // Read user input
            switch (Console.ReadKey().Key) {

                case ConsoleKey.UpArrow:
                    // Move the cursor up
                    cursor = cursor > 0 ? cursor - 1 : nOptions;
                    break;

                case ConsoleKey.DownArrow:
                    // Move the cursor down
                    cursor = cursor < nOptions ? cursor + 1 : 0;
                    break;

                case ConsoleKey.Enter:
                    // Select an option (menu)
                    if (menu == 0)
                        SelectMainOption(cursor, menu, cursor, second);
                    else
                        SelectMainOption(cursor, menu, first, cursor);
                    break;
            }

            // Redraw the menu, if we moved the cursor
            DisplayMultMethods(cursor, menu, first, second);
        }

        // Select an option on the main menu
        private void SelectMainOption(int cursor, int menu = 0, int first = -1, int second = -1) {

            // Check if the user selected (or not) the last option on the current menu
            if (cursor < multMethods.Length) {

                // Check on which menu the user is
                if (menu == 0) {

                    // Go to the second menu
                    DisplayMultMethods(0, 1, first, second);

                } else {

                    // Run matrix multiplication and fo back to the "main" menu
                    CompareMultSpeeds(new int[] {first, second});
                    DisplayMultMethods(0, 0);
                }

            } else {

                // Check on which menu the user is
                if (menu == 0) {

                    // Close the application
                    Environment.Exit(0);

                } else {

                    // Go back to the "main" menu
                    DisplayMultMethods(0, 0);
                }
            }
        }

        // Run 2 multiplications and write results on screen
        private void CompareMultSpeeds(int[] indexes) {

            // Clean the Console
            Console.Clear();

            // Return if both methods are the same
            if (indexes[0] == indexes[1]) {

                // Display message and await user input
                Console.WriteLine("Selected methods are the same! Please choose different methods.");
                Console.WriteLine("\n► 1.Back");
                Console.ReadKey();

                return;
            }

            // Current multiplication info
            double[] operationsTime = new double[indexes.Length];

            // Create necessary matrices as local variables
            Matrix result = new();
            Matrix2D result_2D = new();

            // Set a timer to evaluate performance
            Stopwatch sw = new();
            
            for (int i = 0; i < indexes.Length; i++) {

                Console.WriteLine($"---------- {multMethods[indexes[i]]} ----------\n" + 
                                   "\nCalculating...\n");

                // Restart the timer
                sw.Restart();

                // Multiply matrices by their respective allocated index
                switch (indexes[i]) {

                    case 0:
                        result_2D = Matrix2D.MatMul(m1_2D, m2_2D);
                        break;

                    case 1:
                        result_2D = Matrix2D.MatMulP(m1_2D, m2_2D, THREADS);
                        break;

                    case 2:
                        result = Matrix.MatMulC(m1, m2);
                        break;

                    case 3:
                        result = Matrix.MatMulPC(m1, m2, THREADS);
                        break;

                    case 4:
                        result = Matrix.MatMulT(m1, m2T);
                        break;

                    case 5:
                        result = Matrix.MatMulPT(m1, m2T, THREADS);
                        break;
                }

                operationsTime[i] = sw.Elapsed.TotalMilliseconds;

                // Print the resulting matrix
                if (indexes[i] < 2)
                    Console.WriteLine(result_2D);
                else
                    Console.WriteLine(result);

                Console.WriteLine($"Time: {operationsTime[i]:0.000} ms\n");
                Console.WriteLine($"----------------------------------------------------\n");
            }

            // Stop the timer
            sw.Stop();

            // Show which method was faster and the correspondant performance difference
            if (operationsTime[0] > operationsTime[1])
                Console.WriteLine($"{multMethods[indexes[1]]} was {operationsTime[0] / operationsTime[1]:0.00}x faster than {multMethods[indexes[0]]}!");
            else
                Console.WriteLine($"{multMethods[indexes[0]]} was {operationsTime[1] / operationsTime[0]:0.00}x faster than {multMethods[indexes[1]]}!");

            // Write and wait for user input
            Console.WriteLine("\n► 1.Back");
            Console.ReadKey();
        }
    }
}
