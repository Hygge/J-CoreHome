<template>

    <el-card>

        <el-form
            ref="ruleFormsss"
            :model="ruleForm"
            :rules="rules"
            label-width="120px"
            class="demo-ruleForm"
            size="default"
            status-icon
        >
            <el-form-item label="标题" prop="title">
                <el-input v-model="ruleForm.title"></el-input>
            </el-form-item>
            <el-form-item label="分类" prop="categoryId">
                <el-select v-model="ruleForm.categoryId" placeholder="请选择分类">
                    <el-option  v-for="(item, index) in categories" :key="item.cId" :label="item.categoryName" :value="item.cId"></el-option>
                </el-select>
            </el-form-item>

            <el-form-item label="摘要" prop="desc">
                <el-input type="textarea" v-model="ruleForm.desc" style="width: 50%;"></el-input>
            </el-form-item>
            <el-form-item label="内容" prop="editorData">
                <div style="border: 1px solid #ccc">
                    <Toolbar
                        style="border-bottom: 1px solid #ccc"
                        :editor="editorRef"
                        :defaultConfig="toolbarConfig"
                        :mode="mode"
                    />
                    <Editor
                        style="min-height: 300px; overflow-y: hidden;"
                        v-model="valueHtml"
                        :defaultConfig="editorConfig"
                        :mode="mode"
                        @onCreated="handleCreated"
                        @onChange="handleChange"
                        @onDestroyed="handleDestroyed"
                        @onFocus="handleFocus"
                        @onBlur="handleBlur"
                        @customAlert="customAlert"
                        @customPaste="customPaste"
                    />
                </div>

            </el-form-item>

            <el-form-item>
                <el-button type="primary" @click="submitForm('ruleForm')">立即创建</el-button>
                <el-button @click="resetForm('ruleForm')">重置</el-button>
            </el-form-item>
        </el-form>

    </el-card>


</template>
<script>
import '@wangeditor/editor/dist/css/style.css' // 引入 css
import { onBeforeUnmount, ref, shallowRef, onMounted ,reactive, unref} from 'vue'
import { Editor, Toolbar } from '@wangeditor/editor-for-vue'
import {ElMessage} from "element-plus";
import {Get, PatchJson, Put,} from "@/utils/axios/restful.js";
import {useRouter} from "vue-router";
import {getCategory} from "@/api/common.js";
import {BASE_URL} from "@/utils/axios/index.js";

export default {
    components: { Editor, Toolbar },
    setup() {
        const router = useRouter();
        // 编辑器实例，必须用 shallowRef
        const editorRef = shallowRef()
        // 内容 HTML
        const valueHtml = ref('')

        const toolbarConfig = {}
        const editorConfig =  {
                placeholder: '请输入内容...',
                MENU_CONF: {}
            };
        editorConfig.MENU_CONF['uploadImage'] = {
            server: BASE_URL + '/api/Common/UploadFile',
            // form-data fieldName ，默认值 'wangeditor-uploaded-image'
            fieldName: 'files',

            // 单个文件的最大体积限制，默认为 2M
            maxFileSize: 5 * 1024 * 1024, // 1M

            // 最多可上传几个文件，默认为 100
            maxNumberOfFiles: 10,

            // 选择文件时的类型限制，默认为 ['image/*'] 。如不想限制，则设置为 []
            allowedFileTypes: ['image/*'],

            // 自定义上传参数，例如传递验证的 token 等。参数会被添加到 formData 中，一起上传到服务端。
            // meta: {
            //     token: 'xxx',
            //     otherKey: 'yyy'
            // },

            // 将 meta 拼接到 url 参数中，默认 false
            metaWithUrl: false,

            // 自定义增加 http  header
            headers: {
                Accept: 'application/json',
                token:  localStorage.getItem("token"),

            },
            // 跨域是否传递 cookie ，默认为 false
            withCredentials: true,
            // 超时时间，默认为 10 秒
            timeout: 10 * 1000, // 5 秒
            // 单个文件上传失败
             onFailed(file, res) {           // JS 语法
                console.log(`${file.name} 上传失败`, res)
                 ElMessage(`${file.name} 上传失败`, res)
            },

            // 上传错误，或者触发 timeout 超时
            onError(file, err, res) {               // JS 语法
                console.log(`${file.name} 上传出错`, err, res)
                ElMessage.error(`${file.name} 上传出错 `+err)
            },
        }

        // 组件销毁时，也及时销毁编辑器
        onBeforeUnmount(() => {
            const editor = editorRef.value
            if (editor == null) return
            editor.destroy()
        })

        const handleCreated = (editor) => {
            editorRef.value = editor
            console.log('created', editor)
        }
        const handleChange = (editor) => {
            console.log('change:', editor.getHtml());
            console.log('change:', editor.getText());
        }
        const handleDestroyed = (editor) => {
            // console.log('destroyed', editor)
        }
        const handleFocus = (editor) => {
            // console.log('focus', editor)
        }
        const handleBlur = (editor) => {
            // console.log('blur', editor)
        }
        const customAlert = (info, type) => {
            // alert(`【自定义提示】${type} - ${info}`)
        }
        const customPaste = (editor, event, callback) => {
            console.log('ClipboardEvent 粘贴事件对象', event)
            // const html = event.clipboardData.getData('text/html') // 获取粘贴的 html
            const text = event.clipboardData.getData('text/plain') // 获取粘贴的纯文本
            // const rtf = event.clipboardData.getData('text/rtf') // 获取 rtf 数据（如从 word wsp 复制粘贴）

            // 自定义插入内容
            editor.insertText(text)

            // 返回 false ，阻止默认粘贴行为
            event.preventDefault()
            callback(false) // 返回值（注意，vue 事件的返回值，不能用 return）

            // 返回 true ，继续默认的粘贴行为
            // callback(true)
        }

        const ruleFormsss = ref(null);
        const ruleForm = reactive({
            arId: 0,
            title: '',
            categoryId: '',
            desc: '',
            content: valueHtml,

        });
        const rules = {
            title: [
                {required: true, message: '请输入文章标题', trigger: 'blur'},
                {
                    min: 3,
                    max: 30,
                    message: '长度在 3 到 5 个字符',
                    trigger: 'blur',
                },
            ],
            categoryId: [
                {required: true, message: '请选择分类', trigger: 'change'},
            ],
            desc: [
                {required: true, message: '请填写摘要', trigger: 'blur'},
            ],
        };
        const categories = ref([ ]);
        const submitForm = async () => {
            const form = unref(ruleFormsss);
            console.log(ruleForm.categoryId)
            if (!form) return
            try {
                await form.validate()

                if (arId == 0){
                    Put('/Articles', { title: ruleForm.title, overview: ruleForm.desc, content: ruleForm.content,
                        categoryId: ruleForm.categoryId}).then(res => {
                        if (res.data.code == 200){
                            ElMessage.success("保存成功");
                            resetForm();
                        }
                    })
                }else {
                    PatchJson('/Articles/' + arId,
                         { title: ruleForm.title, overview: ruleForm.desc, content: ruleForm.content,
                        categoryId: ruleForm.categoryId}).then(res => {
                        if (res.data.code == 200){
                            ElMessage.success("修改成功");
                        }
                    })
                }


            } catch (error) {
                ElMessage.error(error);
            }
        }
        const resetForm = ( ) => {
            ruleForm.content = '';
            ruleForm.title = '';
            ruleForm.categoryId = 'net';
            ruleForm.desc = '';
        }

        const arId = router.currentRoute.value.params.id;
        onMounted(() => {
            // 加载分类
            getCategory().then( res => {
                categories.value = res.data.data
            })
            if ( arId != 0){
                // 获取当前路由参数 router.currentRoute.value.params.cId
                Get('/Articles/'+ arId , null).then(res => {
                    ruleForm.arId = res.data.data.arId;
                    ruleForm.title = res.data.data.title;
                    ruleForm.categoryId = res.data.data.categoryId;
                    ruleForm.desc = res.data.data.overview;
                    ruleForm.content = res.data.data.content;
                })
            }

        })

        return {
            editorRef,
            valueHtml,
            mode: 'default', // 或 'simple'
            toolbarConfig,
            editorConfig,
            categories,
            handleCreated,
            submitForm, rules, ruleForm, ruleFormsss, resetForm,
            handleChange,
            handleDestroyed,
            handleFocus,
            handleBlur,
            customAlert,
            customPaste,

        };
    }
}
</script>

<style scoped>
.el-card {
    width: 70%;
    margin: 0 auto;
    border-radius: 30px;
    padding: 30px;

    .el-input {
        width: 50%;
    }


}
.ck-editor__editable {
    min-height: 100px;
    max-height: 500px;
}
</style>