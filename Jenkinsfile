pipeline {
  agent {
    node {
      label 'master'
    }

  }
  stages {
    stage('Build (W node)') {
      steps {
        node(label: 'windows_lorraine') {
          bat(script: 'build.bat', encoding: 'utf-8', returnStatus: true, returnStdout: true)
        }

      }
    }

    stage('Test') {
      steps {
        echo 'test'
      }
    }

  }
}