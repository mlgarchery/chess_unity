pipeline {
  agent any
  stages {
    stage('print') {
      steps {
        sh 'java -version'
        echo 'yooo'
      }
    }

    stage('error') {
      steps {
        bat(script: 'echo.bat', returnStdout: true)
      }
    }

  }
}