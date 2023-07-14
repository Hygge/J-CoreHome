//路由文件
import { createRouter, createWebHistory } from 'vue-router'


const routes = [
    {
        path: '/',
        name: 'index',
        redirect: '/index/home',

    },
    {
        path: '/index',
        name: 'home',
        meta: 'Home',
        component: () => import('@/views/index.vue'),
        children:[
            {
                path: 'home',
                name: 'home',
                meta: 'Home',
                component: () => import('@/views/home/index.vue'),
                //独享守卫
                beforeEnter(to,from,next){
                    next()
                }
            },
            {
                path: 'detail/:arId',
                name: 'detail',
                meta: 'Detail',
                component: () => import('@/views/Article/Detail.vue'),
                //独享守卫
                beforeEnter(to,from,next){
                    next()
                }
            },
            {
                path: 'Categories/:cId',
                name: 'Categories',
                meta: 'Categories',
                component: () => import('@/views/Article/Categories.vue'),
                //独享守卫
                beforeEnter(to,from,next){
                    next()
                }
            },

        ]
    },

    {
        path: '/index1',
        name: 'home1',
        component: () => import('@/views/home/index.vue'),
        //独享守卫
        beforeEnter(to,from,next){
            next()
        }
    },
    {
        path: '/login',
        name: 'login',
        meta: 'login',
        component: () => import('@/views/Login.vue'),
    },
    {
        path: '/console',
        name: 'admin',
        meta: 'admin',
        component: () => import('@/views/admin/Index.vue'),

        children:[
           {
                path: 'index',
                name: 'Home',
                meta: 'Home',
                component: () => import('@/views/admin/Home.vue'),
                //独享守卫
                beforeEnter(to,from,next){
                    auth();
                    next()
                }
            },
           {
                path: 'article',
                name: 'article',
                meta: 'article',
                component: () => import('@/views/admin/Articles.vue'),
                //独享守卫
                beforeEnter(to,from,next){
                    auth();
                    next()
                }
            },
           {
                path: 'userInfo',
                name: 'userInfo',
                meta: 'userInfo',
                component: () => import('@/views/admin/UserInfo.vue'),
                //独享守卫
                beforeEnter(to,from,next){
                    auth();
                    next()
                }
            },
           {
                path: 'fileList',
                name: 'fileList',
                meta: 'fileList',
                component: () => import('@/views/admin/FileList.vue'),
                //独享守卫
                beforeEnter(to,from,next){
                    auth();
                    next()
                }
            },
           {
                path: 'friends',
                name: 'friends',
                meta: 'friends',
                component: () => import('@/views/admin/Friends.vue'),
                //独享守卫
                beforeEnter(to,from,next){
                    auth();
                    next()
                }
            },
           {
                path: 'categories',
                name: 'categories',
                meta: 'categories',
                component: () => import('@/views/admin/Categories.vue'),
                //独享守卫
                beforeEnter(to,from,next){
                    auth();
                    next()
                }
            },
           {
                path: 'settings',
                name: 'settings',
                meta: 'settings',
                component: () => import('@/views/admin/Settings.vue'),
                //独享守卫
                beforeEnter(to,from,next){
                    auth();
                    next()
                }
            },
           {
                path: 'addArticle/:id',
                name: 'addArticle',
                meta: 'addArticle',
                component: () => import('@/views/admin/AddArticle.vue'),
                //独享守卫
                beforeEnter(to,from,next){
                    auth();
                    next()
                }
            },

        ]
    },

    // {
    //     path: '/car',
    //     name: 'car',
    //     component: () => import('../views/Car.vue'),
    //     children:[
    //         {
    //             path: 'car/son1',
    //             name: 'son1',
    //             component: ()=>import("../views/Son1.vue")
    //         }
    //     ]
    // }
]
const router = createRouter({
// createWebHashHistory()是使用hash模式路由
// createWebHistory()是使用history模式路由
    history: createWebHistory(),
    routes
})
//全局守卫
router.beforeEach((to,from,next)=>{

    next()
})

router.beforeResolve((to,from,next)=>{
    next()
})

router.afterEach((to,from)=>{

})
const auth = () => {
    let token = localStorage.getItem("token");
    if (token == null || token === ''){
        router.push("/index/home")
    }
}

export default router;