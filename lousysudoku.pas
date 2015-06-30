{$reference 'sudoku.dll'}

var the_sudoku : Sudoku.Sudoku;
Begin
    Writeln(Sudoku.Debug.TestString());
    Sleep(5000);
    Sudoku.Debug.TestMessage();
    the_sudoku := Sudoku.Interface.CreateSudoku();
    Readln();
End.