param 
(                      
	[array] $AnArray = @()                     
)

if ($AnArray.Length -eq 0)
{
	"Array is Empty"
}
else
{
    foreach($item in $AnArray)
    {
        $item
    }
}