$projectName=$args[0]
$ambiente=$args[1]
write-host "Parametros defaults, Sintaxe customizada: run-consul.ps1 projectName local|dev|tst|hml|prd" 
write-host "Parametros defaults, default: run-consul.ps1 projectName local" 

if ($projectName -eq $null) {
    $projectName = "projectName"
}

if ($ambiente -eq $null) {
    $ambiente = "local"
}

$dockerProjectName=$projectName + '_consul_' + $ambiente
$dockerFileName='docker-comp-consul-' + $ambiente + '.yml'

docker-compose -p $dockerProjectName -f ./devops/consul-server/$dockerFileName down
docker-compose -p $dockerProjectName -f ./devops/consul-server/$dockerFileName up --build -d