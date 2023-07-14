import instace from "@/utils/axios/index.js";

// get 表单提交 /zoos/:id 或 /zoos
const GetForm = (url, param) => instace.get(url, {
    params: param
}).then((response) => {
    console.log(response)
}).catch((error) => {
    console.log(error);
});
export function Get(url, param) {
    return instace({
        url: url,
        method: 'get',
        params: param,
        headers: {
            'token': localStorage.getItem('token')
        },
    })
}

// post 表单提交
export function PostForm (url, param)
{
    return instace({
        url: url,
        method: 'post',
        params: param,
        // header:{
        //     'Content-Type':'application/json'  //如果写成contentType会报错,如果不写这条也报错
        // }
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded'
        },
    });
}



// put 更新全部 json提交  /zoos/:id
export function Put(url, param)
{
    return instace({
        url: url,
        method: 'put',
        data: param,
        header:{
            'Content-Type':'application/json'  //如果写成contentType会报错,如果不写这条也报错
        }
    });
}

// post json提交
export function PostJson (url, param)
{
    return instace.post(url, param, {
        headers: {
            'token': localStorage.getItem('token')
        },
    })
}

// Patch 表单提交 /zoos/:id/animals/:id
export function  Patch  (url, param) {
    return instace({
        url: url,
        method: 'patch',
        params: param,
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded',
            'token': localStorage.getItem('token')
        },
    });
}
export function  PatchJson  (url, param) {
    return instace({
        url: url,
        method: 'patch',
        data: param,
        headers: {
            'Content-Type':'application/json',
            'token': localStorage.getItem('token')
        },
    });
}
// Delete  /zoos/:id/animals/:id
export function  Delete  (url, param) {
    return instace({
        url: url,
        method: 'delete',
        headers: {
            'Content-Type':'application/json',
            'token': localStorage.getItem('token')
        },
    });
}

