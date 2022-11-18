# Matrix Multiplication
A Matrix multiplication comparison focused application.

## 1. Methods Implemented
* Double Index (Single Thread)
* Double Index (Parallel)
* Linear Classic (Single Thread)
* Linear Classic (Parallel)
* Linear Transposed (Single Thread)
* Linear Transposed (Parallel)

## 2. Changing Matrix Settings
Change the following variables in the `Menu.cs` file to control the created matrices:
1. `THREADS`
    * Controls the number of threads used by parallelized methods
2. `M1_ROWS` & `M1_COLS`
    * Control the number of rows and columns of the first matrix
3. `M2_ROWS` & `M2_COLS`
    * Control the number of rows and columns of the second matrix (affects transposed matrix)
4. `M1_FILL` & `M2_FILL`
	* Control if the first and second matrices should be filled with a specific number (setting as `null` fills a matrix with incremented values, starting from `0`)
 
## 3. How to Compare Methods
* Use the `Up` and `Down` arrows on the keyboard to move the cursor;
* Press `Enter` to select the first desired multiplication method;
* Repeat the above processes on the second menu;
* Once on the comparison menu, wait for both methods to complete;
* Observe the results;
* Press `Enter` to return to the first menu.
