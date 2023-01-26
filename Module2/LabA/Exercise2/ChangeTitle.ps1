# Clear the screen.
cls

# Load the SharePoint PowerShell snap-in.
Write-Host "Verify SharePoint PowerShell snap-in is loaded..." -ForegroundColor White
$snapin = Get-PSSnapin | ?{$_.Name -eq 'Microsoft.SharePoint.PowerShell'}
if ($snapin -eq $null) {
    Write-Host "Loading SharePoint PowerShell snap-in..." -ForegroundColor White
    Add-PSSnapin "Microsoft.SharePoint.PowerShell"
}
Write-Host "SharePoint PowerShell snap-in loaded." -ForegroundColor Green

# Get a site collection URL from the user.
Write-Host
$siteUrl = Read-Host "Please provide the site URL"
$site = Get-SPSite -Identity $siteUrl -ErrorAction Ignore
Write-Host

if($site)
{
    Write-Host "Found site: $($site.ID)"
}
else
{
    Write-Host "Unable to find site at $siteUrl"
    break
}

# Display a list of webs in the site.
Write-Host
Write-Host "The site contains the following webs: "

foreach($web in $site.AllWebs)
{
    Write-Host
    Write-Host $web.Title        
    $choice = Read-Host "Edit this web? (Y/N) (N is default)"
    if($choice -eq "y")
    {
        # Update the site title.
        $newTitle = Read-Host "Please provide the new web title, or press Enter to cancel"
        if($newTitle)
        {
            Write-Host "Updating web title..." -ForegroundColor Gray
            $web.Title = $newTitle
            $web.Update()
            Write-Host "Web title updated." -ForegroundColor Green
        }
    }  
    $web.Dispose()  
}