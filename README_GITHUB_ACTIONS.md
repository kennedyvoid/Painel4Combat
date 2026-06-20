# 4Combat - Projeto pronto para GitHub Actions

Este pacote já vem com:

- Projeto `CombatScore.UI`
- Arquivo `4CombatSetup.iss`
- Workflow `.github/workflows/build-installer.yml`

## Como usar

1. Extraia este ZIP.
2. Apague os arquivos antigos do seu repositório, se quiser evitar conflito.
3. Suba todo o conteúdo extraído para o GitHub.
4. Confirme que a estrutura ficou assim:

```text
Painel4Combat/
├── .github/
│   └── workflows/
│       └── build-installer.yml
├── CombatScore.UI/
│   └── CombatScore.UI.csproj
└── 4CombatSetup.iss
```

## Depois do push

1. Vá em `Actions`.
2. Clique em `Build 4Combat Installer`.
3. Aguarde ficar verde.
4. No final da execução, baixe o artifact `4CombatSetup`.
5. Dentro estará o `4CombatSetup.exe`.

## Observação

Se você fizer upload pelo navegador e a pasta `.github` não aparecer, faça pelo terminal ou GitHub Desktop.
Pelo terminal, dentro da pasta extraída:

```bash
git init
git remote add origin URL_DO_SEU_REPOSITORIO
git add .
git commit -m "Projeto 4Combat com instalador automatico"
git branch -M principal
git push -u origin principal
```
