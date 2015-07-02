{$reference 'sudoku.dll'}

uses FormsABC;

var the_sudoku : Sudoku.Sudoku;
Begin
    MainForm.SetSize(500,500);
    Writeln(Sudoku.Debug.TestString());
    Sleep(1000);
    Sudoku.Debug.TestMessage();
    the_sudoku := Sudoku.Interface.CreateSudoku();
End.