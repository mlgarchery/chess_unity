pipeline {
  agent {
    node {
      label 'master'
    }

  }
  stages {
    stage('Master node Print') {
      steps {
        sh 'java -version'
        echo 'yooo'
        pwd(tmp: true)
      }
    }

    stage('Windows Echo') {
      steps {
        node(label: 'windows_lorraine') {
          bat(script: 'build.bat', returnStatus: true, returnStdout: true, encoding: 'utf-8')
        }

      }
    }

  }
}