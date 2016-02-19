library standart;

function Check(value : array of integer; var mask : array of boolean) : array of integer;
begin
    SetLength(result, 0);
    for var i : integer := 0 to value.Length - 1 do
    begin
        for var j : integer := i + 1 to value.Length - 1 do
        begin
            if ((((mask[i]) and (mask[j])) and (value[i] = value[j]))) then
                begin
                    System.Array.Resize(result, result.Length + 2);
                    result[result.Length - 1] := i;
                    result[result.Length - 2] := j;
                end;
        end;    
    end;
    Check := result;
end;

end.