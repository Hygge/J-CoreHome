import axios from "axios";
import {ElMessage} from "element-plus";
import router from "@/router/index.js";

// console.log("import.meta.env", import.meta.env.VITE_API_URL);
export const BASE_URL = 'https://www.zeng164outlook.online';
// export const BASE_URL = 'https://101.35.253.129:5000';

//创建一个新的请求实例instance,instance.的用法和axios.的用法一致，可以使用instance({})、instance.get（）、instance.post()
const instace = axios.create({
    baseURL: 'https://localhost:7124/api',
    // baseURL: BASE_URL + '/api',
    timeout: 10000, //超时时间
    headers: {
        'token': localStorage.getItem("token")
    }
});

//配置请求拦截器,在请求之前的数据处理,比如在请求头添加token,所有的请求都会经过拦截器
instace.interceptors.request.use(
    //config:该参数表示当前请求的配置对象
    (config) => {
        // config.headers = { 'token': localStorage.getItem("token") }
        return config;
    },
    (err) => {

        return Promise.reject(err); //将错误消息挂到promise的失败函数上
    }
);

//配置响应拦截器
// 响应拦截器:在请求响应之后对数据处理，比如:登录失败、请求数据失败的处理
// instance.interceptors.response.use(response=>{l}, err=>{});
// 响应成功:执行回调函数1;响应失败，执行回调函数2
instace.interceptors.response.use(
    (response) => {

        let data = response.data
        switch (data.code) {
            case 401:
                ElMessage.error(data.message);
                router.push("/login")
                break;
            case 424:
                ElMessage.error(data.message);
                break;
            case 500:
                ElMessage.error('服务器繁忙请稍后再试！');
                break;
        }

        return response; //这里的response就是请求成功后的res , response.data即是请求成功后回调函数内的参数res.data
    },
    (err) => {

        ElMessage.error(err.message);

        return Promise.reject(err); //将错误消息挂到promise的失败函数上
    }
);

export default  instace