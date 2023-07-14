import {Get, PostJson} from "@/utils/axios/restful.js";

// 获取所有分类
export function getCategory() {
        return Get('/Category')
}
export function getArticles(data) {
        return PostJson("/Articles", data)
}

