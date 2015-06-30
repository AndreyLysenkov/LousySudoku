{$reference 'lousysudoku.dll'}

Begin
    Writeln(LousySudoku.Debug.TestString());
    Sleep(5000);
    LousySudoku.Debug.TestMessage();
    Readln();
End.