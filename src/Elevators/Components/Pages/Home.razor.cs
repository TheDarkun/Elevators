﻿namespace Elevators.Components.Pages;

public partial class Home
{
    [Inject] 
    public IConfiguration Configuration { get; set; } = null!;
}