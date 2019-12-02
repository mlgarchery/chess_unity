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
          bat(script: 'docker cp jenkins_container:/var/jenkins_home/workspace/chess_unity_master/ C:\\Users\\martial\\Documents\\workspace\\', encoding: 'utf-8', returnStatus: true, returnStdout: true)
          bat(script: 'build.bat', encoding: 'utf-8', returnStatus: true, returnStdout: true)
        }

      }
    }

  }
}