async function postJSONData(url = '', data = {}) {
    const response = await fetch(url, {
      method: 'POST', 
      cache: 'no-cache', 
      headers: {
        'Accept': 'application/json, */*',
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(data) 
    });
    return response.json(); 
  }

  async function del(url = '') {
    const response = await fetch(url, {
      method: 'DELETE'
    });
    return response.json(); 
  }

  async function patch(url = '', data = {}) {
    const response = await fetch(url, {
      method: 'PATCH', 
      cache: 'no-cache', 
      headers: {
        'Accept': 'application/json, */*',
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(data) 
    });
    return response.json(); 
  }
  export {postJSONData, del, patch}