pipeline {
    agent any

    environment {
        REMOTE_USER = 'janus'
        REMOTE_HOST = '192.168.15.86'
        IMAGE_NAME  = 'hr-automation-api'
    }

    stages {
        stage('Build') {
            steps {
                bat 'dotnet build HR-AUTOMATION-API/HR-AUTOMATION-API.csproj'
            }
        }

       stage('Deploy DEV') {
           when { expression { env.GIT_BRANCH == 'origin/main-dev' } }
           steps {
               bat "docker build -t %REMOTE_HOST%:5002/%IMAGE_NAME%:dev -f HR-AUTOMATION-API/Dockerfile ."
               withCredentials([
                   usernamePassword(credentialsId: 'registry-creds', usernameVariable: 'REG_USER', passwordVariable: 'REG_PASS'),
                   string(credentialsId: 'hr-api-connection-string', variable: 'DB_CONN')
               ]) {
                   bat "docker login %REMOTE_HOST%:5002 -u %REG_USER% -p %REG_PASS%"
                   bat "docker push %REMOTE_HOST%:5002/%IMAGE_NAME%:dev"
                   bat "ssh -o StrictHostKeyChecking=no %REMOTE_USER%@%REMOTE_HOST% \"docker login %REMOTE_HOST%:5002 -u %REG_USER% -p %REG_PASS% && docker pull %REMOTE_HOST%:5002/%IMAGE_NAME%:dev && docker stop hr-api-dev; docker rm hr-api-dev; docker run -d --name hr-api-dev -p 7001:8080 -e ASPNETCORE_ENVIRONMENT=Development -e ConnectionString='%DB_CONN%' %REMOTE_HOST%:5002/%IMAGE_NAME%:dev\""
               }
           }
       }

        stage('Deploy QA') {
            when { expression { env.GIT_BRANCH == 'origin/main-qa' } }
            steps {
                bat "docker build -t %REMOTE_HOST%:5001/%IMAGE_NAME%:qa -f HR-AUTOMATION-API/Dockerfile ."
                withCredentials([usernamePassword(credentialsId: 'registry-creds', usernameVariable: 'REG_USER', passwordVariable: 'REG_PASS')]) {
                    bat "docker login %REMOTE_HOST%:5001 -u %REG_USER% -p %REG_PASS%"
                    bat "docker push %REMOTE_HOST%:5001/%IMAGE_NAME%:qa"
                    bat "ssh -o StrictHostKeyChecking=no %REMOTE_USER%@%REMOTE_HOST% \"docker login %REMOTE_HOST%:5001 -u %REG_USER% -p %REG_PASS% && docker pull %REMOTE_HOST%:5001/%IMAGE_NAME%:qa && docker stop hr-api-qa; docker rm hr-api-qa; docker run -d --name hr-api-qa -p 7002:8080 %REMOTE_HOST%:5001/%IMAGE_NAME%:qa\""
                }
            }
        }

        stage('Deploy PROD') {
            when { expression { env.GIT_BRANCH == 'origin/main-prod' } }
            steps {
                input message: '¿Confirmar deploy a producción?', ok: 'Deployar'
                bat "docker build -t %REMOTE_HOST%:5000/%IMAGE_NAME%:latest -f HR-AUTOMATION-API/Dockerfile ."
                withCredentials([usernamePassword(credentialsId: 'registry-creds', usernameVariable: 'REG_USER', passwordVariable: 'REG_PASS')]) {
                    bat "docker login %REMOTE_HOST%:5000 -u %REG_USER% -p %REG_PASS%"
                    bat "docker push %REMOTE_HOST%:5000/%IMAGE_NAME%:latest"
                    bat "ssh -o StrictHostKeyChecking=no %REMOTE_USER%@%REMOTE_HOST% \"docker login %REMOTE_HOST%:5000 -u %REG_USER% -p %REG_PASS% && docker pull %REMOTE_HOST%:5000/%IMAGE_NAME%:latest && docker stop hr-api-prod; docker rm hr-api-prod; docker run -d --name hr-api-prod -p 7000:8080 %REMOTE_HOST%:5000/%IMAGE_NAME%:latest\""
                }
            }
        }
    }

    post {
        success { echo "✅ Pipeline exitoso" }
        failure  { echo "❌ Falló el pipeline" }
    }
}