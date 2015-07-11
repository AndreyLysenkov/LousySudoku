{$reference 'core.dll'}

uses 
    System,
    FormsABC,
    LousySudoku;

const
    directory : string = 'data\\templates\\';
    extension : string = 'Text file | *.txt';
    windowTitle : string = 'LousySudoku v1';
    complexityDefault : double = 0.27;
    
var
    theSudoku : Sudoku;
    numb : array [,] of integer;
    mask : array [,] of integer;
    rightness : array [,] of boolean;
    filenameTextBox : TextBox;
    complexityTextBox : TextBox;
    cell : array of array of TextBox;

procedure GetSudokuGrid();
begin
    theSudoku.GetGrid(numb, mask, rightness);
end;

procedure EnterValues();
begin
    for var i : integer := 0 to theSudoku.Size.X - 1 do
    begin
        for var j : integer := 0 to theSudoku.Size.Y - 1 do
        begin
            var curPosition : Number.Position
                := new Number.Position(i, j);
            var cellNumber : Number :=
                theSudoku.GetNumber(curPosition);
            if (cellNumber.IsExist) and not(cell[i, j].Text = '')                
            then begin
                try
                begin
                    cellNumber.Modify(Convert.ToInt32(cell[i, j].Text));
                end
                except begin
                    cellNumber.Modify(0);
                    cell[i, j].Text := '';
                end;
                end;
            end;
        end;
    end;
end;

procedure RefreshGrid();
begin
    for var i : integer := 0 to theSudoku.Size.X - 1 do
    begin
        for var j : integer := 0 to theSudoku.Size.Y - 1 do
        begin
            var curPosition : Number.Position
                := new Number.Position(i, j);
            var cellNumber : Number :=
                theSudoku.GetNumber(curPosition);
            if (cellNumber.IsExist) 
            then begin
                cell[i, j].Text := cellNumber.Value.ToString();
            end;
            if not(cellNumber.IsRight)
            then begin
                ///cell[i, j].Text := cell[i, j].Text;//String.Format('-{0}-', cell[i, j].Text);
                cell[i, j].Height := 20;
            end;
            if (cellNumber.Type = Number.NumberType.Empty)
            then begin
                cell[i, j].Text := ' ';
            end;
        end;
    end;
end;

procedure ShowSudoku();
begin
    var buttonCheck : Button := new Button('Check sudoku');        
    buttonCheck.Click += buttonOnCheck;
    LineBreak;
    GetSudokuGrid();
    var lengthX : integer := theSudoku.Size.X;
    var lengthY : integer := theSudoku.Size.Y;    
    System.Array.Resize(cell, lengthX);
    for var i : integer := 0 to lengthX - 1 do
    begin
        System.Array.Resize(cell[i], lengthY);
        for var j : integer := 0 to lengthY - 1 do
        begin
            var curPosition : Number.Position
                := new Number.Position(i, j);
            var cellNumber : Number :=
                theSudoku.GetNumber(curPosition);
            if (cellNumber.IsExist)                
            then begin
                cell[i, j] := new TextBox();
                cell[i, j].Width := 27;
                cell[i, j].Height := 27;
            end else begin
                var space := new Space(27); 
            end;
        end;
        LineBreak;
    end;
    MainForm.Width := 50 + 40 * lengthX;
    MainForm.height := 50 + 40 * lengthY;
    RefreshGrid();
    MainForm.IsFixedSize := false;
end;

procedure LoadFileName(filename : string; complexity : double);
begin
    theSudoku := LousySudoku.Interface.GenerateFromTemplate(filename, complexity);
end;

procedure ClickOk();
begin
    LoadFileName(
        filenameTextBox.Text, 
        Convert.ToDouble(complexityTextBox.Text)
    );
    filenameTextBox.Text := String.Format('Loaded: "{0}"', filenameTextBox.Text);
    ShowSudoku();       
end;

function PutTextBox(boxLabel : string; boxText : string; boxHeight : integer; boxWidth : integer) : TextBox;
begin
    var thLabel : TextLabel := new TextLabel(boxLabel);
    var textBox : TextBox := new TextBox();
    textBox.Text := boxText;
    textBox.Height := boxHeight;
    textBox.Width := boxWidth;
    Result := textBox;
end;

procedure OpenFile(obj : object);
begin
    
end;

procedure FileChooseWindow();
begin
    MainForm.IsFixedSize := true;
    MainForm.Height := 150;
    MainForm.Width := 400;    
    filenameTextBox := PutTextBox(
            'Template filename: ', 
            directory + extension,
            24,
            270
        );
    complexityTextBox := PutTextBox(
            'Complexity [0-1]: ', 
            complexityDefault.ToString(),
            24,
            50
        );
    var buttonOk : Button := new Button('Generate sudoku');        
    buttonOk.Click += ClickOk;
end;

begin
    MainForm.Title := windowTitle;
    FileChooseWindow();
end.