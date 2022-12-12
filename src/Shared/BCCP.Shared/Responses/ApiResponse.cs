﻿namespace BCCP.Shared.Responses;

public class ApiResponse
{
    public bool IsSuccess { get; set; } = true;
    public object Result { get; set; }
    public string DisplayMessage { get; set; } = "";
    public List<string> ErrorMessages { get; set; }
}
