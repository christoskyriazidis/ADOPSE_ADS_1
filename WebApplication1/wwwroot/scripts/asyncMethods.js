class Ajax {
    static request (method, url, data = null) {
      return new Promise((resolve, reject) => {
        let xhr = new XMLHttpRequest()
        xhr.open(method, url, true)
        xhr.setRequestHeader('Content-Type', 'application/json')
        xhr.onload = () => {
          if (xhr.status === 200) {
            return resolve(JSON.parse(xhr.responseText || '{}'))
          } else {
            return reject(new Error(`Request failed with status ${xhr.status}`))
          }
        }
        if (data) {
          xhr.send(JSON.stringify(data))
        } else {
          xhr.send()
        }
      })
    }
  }

class Axios{

  
}
