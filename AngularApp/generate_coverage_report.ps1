param(
    [switch]$w,
    [switch]$d
)

# Função para matar processos com base no nome do processo
function Stop-ProcessesByName {
  $processes = Get-Process | Where-Object { $_.ProcessName -like 'npm*' -or $_.ProcessName -like '*job*' } | Where-Object { $_.MainWindowTitle -eq '' }
  if ($processes.Count -gt 0) {
      $processes | ForEach-Object { Stop-Process -Id $_.Id -Force }
  }
}

function Open-Report-Coverage {
  # Encontra o diretório mais recente se ele existir
  $latestDir = Get-ChildItem -Directory -Path $reportPath | Sort-Object LastWriteTime -Descending | Select-Object -First 1

  # Verifica se encontrou um diretório e, em caso afirmativo, obtém o nome do diretório (GUID)
  if ($null -ne $latestDir) {
    # Abre a página index.html no navegador padrão do sistema operacional
    Invoke-Item $reportPath\index.html
  }
  else {
    Write-Host "Nenhum diretório de resultados encontrado."
  }
}

# Executa os testes unitários e gera o relátorio
npm run test:coverage

# Path onde é gerado o relatório coverage
$reportPath = ".\coverage\lcov-report"


# Encerra qualquer processo em segundo plano relacionado ao comando npm run test:watch
Stop-ProcessesByName

if ($w) {
  # Executa os testes unitários em modo de observação
  Start-Job -ScriptBlock { npm run test:watch }
  Open-Report-Coverage
}
else {
  if ($d) {
    # Executa os testes unitários em modo de observação no Chrome junto com karma
    Start-Job -ScriptBlock { npm run test:watch }
    Open-Report-Coverage
    npm run test:debug
  }
  else{
    Open-Report-Coverage
  }
}

Get-Process | Where-Object { $_.ProcessName -like 'npm*' -or $_.ProcessName -like '*job*' } | Where-Object { $_.MainWindowTitle -eq '' }
