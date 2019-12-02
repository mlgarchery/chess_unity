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
        bat(script: 'tools\\echo.bat', returnStdout: true)
      }
    }

  }
}