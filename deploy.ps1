include .\_powerup\deploy\combos\PsakeCombos\StandardSettingsAndRemoteExec.ps1

task deploy {
	RunTask deploy-website ${execute.remotely} ${website.server}
}

function RunTask($task, $executeRemotely, $server) {
  if(ConvertTo-Bool(${execute.remotely})) {
      run $task $server
  } else {
      invoke-task $task
  }
}

function set-RequiresSsl($appPath, $value) {	
	set-WebConfigurationPropertyIfRequired "/system.webServer/security/access" "sslFlags" $value $appPath
}

#This function is in powerupweb, but it's private.  I didn't want to modify powerup just to expose this function
function set-WebConfigurationPropertyIfRequired($xpath, $propertyName, $value, $appPath) {
	try
	{
		$existingValue = get-webconfigurationproperty -Filter $xpath -name $propertyName -PSPath IIS:\ -Location $appPath	
	}catch{}
	
	write-host $xpath $propertyName $value $appPath $existingValue
	
	if($existingValue -ne $value)
	{
		write-host "setting value $xpath $propertyName $value"
		Set-WebConfigurationProperty -Filter $xpath -name $propertyName -Value $value -PSPath IIS:\ -Location $appPath	
	}
}

task deploy-website {
	$webComboOptions = @{
		websitename = ${website.name};
		webroot = ${websites.deployment.root};
		sourcefolder="Web";	
	}
	
	if ($(ConvertTo-Bool ${website.use.ssl})) {
		$webComboOptions.bindings = @(
					@{
						port = ${website.https.port};
						protocol="https";
						useselfsignedcert=$(ConvertTo-Bool ${website.use.selfsignedcert});
						certname=${website.cert.name};
					},
					@{
						port = ${website.http.port};
					}
				);
				
		if (${website.hostname}) {
			$webComboOptions.bindings[0].url = ${website.hostname}
			$webComboOptions.bindings[1].url = ${website.hostname}
		}
		
		if (${website.ip}) {
			$webComboOptions.bindings[0].ip = ${website.ip}
			$webComboOptions.bindings[1].ip = ${website.ip}
		}
		
	} else {
		$webComboOptions.bindings = @(
			@{port = ${website.http.port}}
		);
				
		if (${website.hostname}) {
			$webComboOptions.bindings[0].url = ${website.hostname}
		}
		
		if (${website.ip}) {
			$webComboOptions.bindings[0].ip = ${website.ip}
		}
	}	
	
	if(${website.apppool.username}) {
		$webComboOptions.apppool = @{username = ${website.apppool.username}; password = ${website.apppool.password}};
	}
	elseif(${website.apppool.identity}) {
		$webComboOptions.apppool = @{identity = ${website.apppool.identity}; };
	}

	invoke-combo-standardwebsite $webComboOptions
}