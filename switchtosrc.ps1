$binPath='submodules\MvvmCross-Binaries\VS2012\bin'
$srcPath='submodules\MvvmCross\bin'

ls *.csproj -rec | 
	%{ $f=$_; (gc $f.PSPath) | 
		%{ $_ -replace [regex]::Escape($binPath), $srcPath } | 
		sc $f.PSPath 
	}