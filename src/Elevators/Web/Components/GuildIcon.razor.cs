﻿using Microsoft.AspNetCore.Components;

namespace Elevators.Web.Components;

public partial class GuildIcon
{
    [Parameter] 
    public Guild Guild { get; set; } = null!;
    
    [Parameter]
    public string Class { get; set; } = string.Empty;
    
    private bool ImageLoadFailed { get; set; } = false;

    private void HandleImageError()
        => ImageLoadFailed = true;
    
    [Parameter]
    public EventCallback<ulong> OnImageClick { get; set; }


}