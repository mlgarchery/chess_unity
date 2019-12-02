pipeline {
  agent any
  stages {
    stage('print') {
      steps {
        sh 'echo 1'
        echo 'yooo'
      }
    }

    stage('') {
      steps {
        bat(script: 'echo.bat', returnStdout: true)
      }
    }

  }
}