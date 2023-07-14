import { defineConfig } from 'vite'
import { resolve } from 'path'
import vue from '@vitejs/plugin-vue'


//import ElementPlus from 'unplugin-element-plus/vite'


// https://vitejs.dev/config/

export default defineConfig({
  plugins: [vue(),
  //  ElementPlus()

  ],
  // 起个别名，在引用资源时，可以用‘@/资源路径’直接访问
  resolve: {
    alias: {
      "@": resolve(__dirname, "src"),

    },
  },
  // 配置前端服务地址和端口
  server: {
    host: '0.0.0.0',
    cors: true,
    port: 3000,
    // 是否开启 https
    https: false,
    proxy: {
      '/api': {
        // 后台地址
        target: 'https://www.zeng164outlook.online/api',
        changeOrigin: true,
        secure: true,
        rewrite: path => path.replace(/^\/api/, '')
        // pathRewrite: {
        //   '^/api1': ''
        // }
      },
    }

  },


})
