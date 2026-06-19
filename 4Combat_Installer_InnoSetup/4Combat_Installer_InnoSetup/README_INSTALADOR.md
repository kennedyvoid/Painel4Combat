# Instalador do 4Combat - Inno Setup

## Como gerar o instalador

### 1. Instale o Inno Setup
Baixe e instale o Inno Setup no Windows.

### 2. Publique o projeto no Visual Studio
No Visual Studio:

1. Clique com botão direito no projeto `CombatScore.UI`
2. Clique em `Publish` / `Publicar`
3. Escolha `Folder` / `Pasta`
4. Configuração recomendada:
   - Configuration: `Release`
   - Target runtime: `win-x64`
   - Deployment mode: `Self-contained`

Isso faz o app funcionar nas máquinas mesmo sem instalar .NET separado.

### 3. Copie os arquivos publicados
Copie todo o conteúdo da publicação para a pasta:

```text
4Combat_Installer_InnoSetup/publish
```

A pasta deve conter:

```text
CombatScore.UI.exe
.dll
.runtimeconfig.json
.deps.json
Assets
```

### 4. Gere o instalador
Abra o arquivo:

```text
4CombatSetup.iss
```

no Inno Setup e clique em:

```text
Build > Compile
```

### 5. Resultado
O instalador será gerado em:

```text
installer-output/4CombatSetup.exe
```

Esse é o arquivo que você pode colocar no Google Drive, OneDrive, GitHub Releases ou pen drive para instalar nas máquinas.