param 
(                      
	[hashtable]$AHashTable = @{}                     
)

foreach($kvp in $AHashTable.GetEnumerator())
{
    $kvp.Key + " = " + $kvp.Value
}