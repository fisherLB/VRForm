var userAndPermission = [];
var userPermission = null;//用户权限（包括浏览权限）
var IsSuperManage = false;   //是否是超级管理员
$(function () {
    if (!Array.prototype.indexOf) {
        Array.prototype.indexOf = function (searchElement, fromIndex) {
            var k;
            if (this == null) {
                throw new TypeError('"this" is null or not defined');
            }
            var O = Object(this);
            var len = O.length >>> 0;
            if (len === 0) {
                return -1;
            }
            var n = +fromIndex || 0;
            if (Math.abs(n) === Infinity) {
                n = 0;
            }
            if (n >= len) {
                return -1;
            }
            k = Math.max(n >= 0 ? n : len - Math.abs(n), 0);
            while (k < len) {
                if (k in O && O[k] === searchElement) {
                    return k;
                }
                k++;
            }
            return -1;
        };
    }
})
$(function () {
    loadMenus();
    getUserPermission();
    userAndPermission = loadUserAndPermission(userPermission);
   
});
//根据formname获取json格式的数据
function getJsonForFrom(formname) {
    var formData = $("#" + formname).serializeArray();
    var json = convertArray(formData);
    delete json.undefined;
    json = JSON.stringify(json);
    return json;
}

//验证输入是否复合长度
function ValidateInputLength(data, length) {
    if (data == "" || data == null || data == undefined)
        return false;
    if (data.length > length)
        return false;
    if (data.length < 0)
        return false;
    return true;
}

//主要是推荐这个函数。它将jquery系列化后的值转为name:value的形式。
function convertArray(o) {
    var v = {};
    for (var i in o) {
        if (o[i].name != '__VIEWSTATE') {
            if (typeof (v[o[i].name]) == 'undefined')
                v[o[i].name] = $.trim(o[i].value);
            else
                v[o[i].name] += "," + $.trim(o[i].value);
        }
    }
    return v;
}
//正则表达式验证输入内容是否符合类型
function ValidateInputType(data, regexe) {
    var pattern = new RegExp(regexe);
    if (data != "" && data != null) {
        if (pattern.test(data))
            return false;

        return true;
    }
    return false;
}

//同步的方式获取用户权限
function getUserPermission() {
    $.ajax({
        type: "get",
        url: "/api/PermissionManage/GetUserPermissionBySession?t=" + Math.random(),
        dataType: "json",
        async: false,
        success: function (d) {
            d = eval('(' + d + ')');
            userPermission = d.Data;
            if (d.Message == "当前登录用户为超级管理员") {
                IsSuperManage = true;
            }
        }
    });
}
    //}

    //检查用户操作权限
    function checkUserAndPermission(url) {
        url = url.toLocaleLowerCase();
        //超级管理员,加载所有控件
        if (IsSuperManage) {
            return true;
        } else {
            //不是超级管理员检查，是否拥有控件权限
            if (userAndPermission[url]) {
                return true;
            }
            return false;
        }

    }

    function loadUserAndPermission(arr) {
        var arry = {};
        $.each(arr, function (i, n) {
            try {
                arry[n.Full_url.toLocaleLowerCase()] = n.Func_grade;
            } catch (e) {

            }
        });

        return arry;
    }

    //用这个方法运行action方法 传入对象格式的数据 传入的路径要全路径 如：{url:'/a/a/c',data:data,successRollback:function(){func();}}
    //url必传
    var processIndex;//进度条角标
    function operationAction(json) {
        if (json.data == undefined) {
            json.data = "";
        }
        if (json.dataType == undefined || json.dataType == "") {
            json.dataType = "get";
        }
        if (json.type == undefined || json.type == "") {
            json.type = "get";
        }
        if (json.async == undefined || json.async == "") {
            json.async = true;
        }
        if (json.successRollback == undefined || json.successRollback == "") {
            json.successRollback = function () { }
        }
        if (json.errorRollback == undefined || json.errorRollback == "") {
            json.errorRollback = function () { }
        }
        var url = json.url, data = json.data, type = json.type, dataType = json.dataType, async = json.async, successRollback = json.successRollback, errorRollback = json.errorRollback;

        processIndex = layer.load(1, {
            shade: [0.1, '#fff'] //0.1透明度的白色背景

        });
        var title = "";
        $.ajax({
            url: '/api/PermissionManage/GetUserActionGrade',
            method: 'get',
            contentType: 'application/json',
            //data: JSON.stringify(url),
            async: async,
            data: { url: url },
            success: function (res) {
                var res = eval('(' + res + ')');
                if (res.Data == "1") {
                    //该用户的权限时直接拥有该功能的权限
                    $.ajax({
                        url: url,
                        type: type,
                        contentType: 'application/json',
                        data: JSON.stringify(data),
                        async: true,
                        success: function (res) {
                            var d = eval('(' + res + ')');
                            if (d.Success) {
                                if (successRollback) {
                                    successRollback(d);
                                    layer.close(permissionIndex);
                                    layer.close(processIndex);
                                }
                            } else {
                                if (d.Data == "-55") {
                                    layer.alert(d.Message, {
                                        title: '提示',
                                        icon: 2,
                                        yes: function (index) {
                                            layer.close(index);
                                        }
                                    });
                                    layer.close(processIndex);
                                } else {
                                    layer.alert(d.Message, {
                                        title: '提示',
                                        icon: 2,
                                        yes: function (index) {
                                            layer.close(index);
                                        }
                                    });
                                }

                            }
                        },

                    });
                    layer.close(processIndex);
                } else if (res.Data == "2") {
                    //该功能需要身份验证才能操作
                    permissionIndex = layer.open({
                        title: '该功能需要身份验证才能操作',
                        type: 1,
                        area: ['400px', '200px'],
                        btn: ['确定', '取消'],
                        btnAlign: 'c',
                        content: $('#PermissionWin').html(),
                        yes: function (index) {
                            submitAuthentication(url, data, dataType, type, async, successRollback, errorRollback);
                        },
                        cancel: function (index) {
                            layer.close(index);
                        }
                    })
                } else if (res.Data == "3") {
                    //上级授权
                    superiorIndex = layer.open({
                        title: '该功能需要上级授权才能操作',
                        type: 1,
                        area: ['400px', '200px'],
                        btn: ['确定', '取消'],
                        btnAlign: 'c',
                        content: $('#SuperiorWin').html(),
                        yes: function (index) {
                            submitAuthoritiesHigher(url, data, dataType, type, async, successRollback, errorRollback);
                        },
                        cancel: function (index) {
                            layer.close(index);
                        }
                    })
                } else if (red.Data == "0") {
                    //用户没有权限 
                    window.location.href = "/Admin/Login"
                } else {
                    layer.alert('抱歉没有该功能的权限！', { icon: 6 })
                }
            }
        });

    }

    //提交身份验证(operationAction)
    var permissionIndex; //身份验证弹框角标
    function submitAuthentication(url, data, dataType, type, async, successRollback, errorRollback) {
        var pwd = $('#Password').val();
        if (pwd == "") {
            layer.alert('请输入密码!');
            return;
        }

        if (!ValidateInputLength(pwd, 12)) {
            layer.alert('密码长度不能大于12位!');
            return;
        }
        if (data == null || data == "") {
            data = "filter_pwd=" + pwd;
        } else {
            data = JSON.stringify(data);
            data += "&filter_pwd=" + pwd;
        }

        $.ajax({
            url: url,
            type: type,
            contentType: 'application/json',
            data: JSON.stringify(data),
            async: true,
            success: function (res) {
                var type = typeof (res);
                var d = new Object();
                if (type == "object")
                    d = res;
                if (type == "string")
                    d = eval('(' + res + ')');
                if (d.Success) {
                    if (successRollback) {
                        successRollback(d);
                        layer.close(permissionIndex);
                        layer.close(processIndex);
                    } else {
                        //addTab(tabTitle, url);
                    }
                } else {
                    if (d.Data == "-55") {
                        layer.alert(d.Message);
                        layer.close(processIndex);
                    }
                }
            },
            error: function (d) { errorRollback(d); layer.close(processIndex); }
        });
    }

    //提交上级授权()
    var superiorIndex;//上级授权弹框角标
    function submitAuthoritiesHigher(url, data, dataType, type, async, successRollback, errorRollback) {
        var username = $('#Superior_name').val();
        var pwd = $('#Superior_pwd').val();
        if (username == "") {
            layer.alert('用户名不能为空！');
            return;
        }
        if (pwd == "") {
            $.messager.alert('提示', '请输入密码', 'info');
            return;
        }

        //if (!ValidateInputType($.trim(username), "[^\a-\z\A-\Z0-9]")) {
        //    layer.alert('用户名不能空！仅限输入英文或数字的组合!');
        //    return;
        //}

        //if (!ValidateInputLength($.trim(username), 12)) {
        //    layer.alert('用户名长度不能大于12位!');
        //    return;
        //}

        //if (!ValidateInputLength(pwd, 12)) {
        //    layer.alert('密码长度不能大于12位!');
        //    return;
        //}
        if (data == null || data == "") {
            data = "filter_pwd=" + pwd + "&filter_username=" + username;
        } else {
            data = JSON.stringify(data);
            data += "&filter_pwd=" + pwd + "&filter_username=" + username;
        }

        $.ajax({
            url: url,
            type: type,
            contentType: 'application/json',
            data: JSON.stringify(data),
            async: true,
            success: function (res) {
                var type = typeof (res);
                var d = new Object();
                if (type == "object")
                    d = res;
                if (type == "string")
                    d = eval('(' + res + ')');
                if (d.Success) {
                    if (successRollback) {
                        successRollback(d);
                        layer.close(superiorIndex);
                        layer.close(processIndex);
                    } else {
                        //addTab(tabTitle, url);
                    }
                } else {
                    if (d.Data == "-55") {
                        layer.alert(d.Message);
                        layer.close(processIndex);
                    }
                }
            },
        });

        //$.ajax({
        //    url: url,
        //    data: data,
        //    dataType: dataType,
        //    type: type,
        //    async: async,
        //    success: function (d) {
        //        if (d.Success) {
        //            $('#authorities_window').window('close');
        //            successRollback(d);
        //            $.messager.progress('close');
        //        } else {
        //            if (d.Data == "-55") {
        //                $.messager.progress('close');
        //                $.messager.alert('提示', d.Message, 'info');
        //            } else {
        //                $('#authorities_window').window('close');
        //                $.messager.progress('close');
        //                successRollback(d);
        //            }
        //        }
        //    },
        //    error: function (d) { errorRollback(d); $.messager.progress('close'); }
        //});
    }


    //获取表单值
    function getValueAndValidate(id) {
        var value = $.trim(document.getElementById(id).value);
        return value;
    }
    //获取机构,新增修改弹框下拉框用
    //parentID  显示的输入框
    //id   生成树节点的id
    //value  隐藏域的input的id
    function getUnitTree(parentID, id, value, fun) {
        $.ajax({
            type: "get",
            url: '/api/UnitManage/GetUnitCombotree',
            dataType: "json",
            success: function (result) {
                if (result != "") {
                    $('#' + id).treeview({
                        data: JSON.parse(result),         // 数据源
                        highlightSelected: true,    //是否高亮选中
                        multiSelect: false,    //多选
                        levels: 10,
                        onhoverColor: '#a1c8ea',
                        expandIcon: 'tree_icon_open',
                        collapseIcon: 'tree_icon_close',
                        //nodeIcon: 'tree_icon_file',
                        //emptyIcon: 'tree_icon_nofile',
                        nodeSelected: function (event, node) {

                        },
                        onNodeExpanded: function (event, data) { },

                        onNodeSelected: function (event, data) {

                        }

                    });
                    $('#' + id).on('nodeSelected', function (event, data) {
                        $('#' + parentID).val(data.text);
                        $('#' + value).val(data.id);
                        $('#' + id).hide();
                        if (fun) {
                            fun();
                        }
                    });
                    $('#' + id).on('nodeUnselected', function (event, data) {
                        $('#' + parentID).val(data.text);
                        $('#' + value).val(data.id);
                        $('#' + id).hide();
                    });
                }
            }
        });
        $('#' + parentID).click(function () {
            $('#' + id).toggle();
        });
        if (!fun) {
            $(document).click(function (event) {
                var _con = $('#' + id);
                if ($(event.target).attr('class').indexOf('expand-icon') != -1) {
                    return;
                }
                if (!_con.is(event.target) && _con.has(event.target).length === 0 && event.target.id != parentID) {
                    $('#unitTree').hide();
                }
            });
        }
    }

    //系统导航栏
    function loadMenusbak() {
        $.ajax({
            type: "get",
            url: "/api/MenuManage/GetMemoryMenuList",
            success: function (data) {
                var ed = eval('(' + data + ')');
                if (ed != null && ed != undefined && ed.rows != null && ed.rows != undefined && ed.rows.length != 0) {
                    var meunList = [];
                    for (var i = 0; i < ed.rows.length; i++) {
                        if (ed.rows[i].ParentId == "") {
                            ed.rows[i].children = [];
                            meunList.push(ed.rows[i]);
                        }
                    }
                    for (var k = 0; k < meunList.length; k++) {
                        for (var j = 0; j < ed.rows.length; j++) {
                            if (ed.rows[j].ParentId != "" && ed.rows[j].ParentId == meunList[k].Func_id) {
                                meunList[k].children.push(ed.rows[j]);
                            }
                        }
                    }
                    InitMenu(meunList);
                }
            }
        })
    }

    function loadMenus() {
        $.ajax({
            type: "get",
            url: "/api/MenuManage/GetMemoryMenuList",
            success: function (data) {

                var ed = eval('(' + data + ')');
                if (ed != null && ed != undefined) {
                    //var meunList = [];
                    //$.each(ed, function (i, n) {

                    //});
                    //for (var i = 0; i < ed.rows.length; i++) {
                    //    if (ed.rows[i].ParentId == "") {
                    //        ed.rows[i].children = [];
                    //        meunList.push(ed.rows[i]);
                    //    }
                    //}
                    //for (var k = 0; k < meunList.length; k++) {
                    //    for (var j = 0; j < ed.rows.length; j++) {
                    //        if (ed.rows[j].ParentId != "" && ed.rows[j].ParentId == meunList[k].Func_id) {
                    //            meunList[k].children.push(ed.rows[j]);
                    //        }
                    //    }
                    //}
                    InitMenu(ed);
                }
            }
        })
    }

    function InitMenu(data) {
        var menulist = "";
        var indexTitle = '启动检测';
        $.each(data, function (i, n) {
            if (i == 0) {
                menulist += "<li class='start-up'><span>" + n.menuname + "</span>";
            } else if (i != 0 && n.children.length == 0) {
                menulist += "<li><span class='navClick' id='" + n.menuid + "' dataurl='" + n.Action + "'datatext=" + n.menuname + " onclick='onClickUrl(this);'>" + n.menuname + "</span>";
            } else {
                menulist += "<li><span>" + n.menuname + "<span class='caret'></span></span>";
            }
            if (n.children.length > 0) {
                menulist += "<ul>";
                $.each(n.children, function (index, item) {
                    menulist += "<li><span class='navClick' id='" + item.id + "' dataurl='" + item.Action + "'datatext=" + item.text + " onclick='onClickUrl(this);' >" + item.text + "</span></li>";
                });
                menulist += "</ul>";
            }
            menulist += "</li>";
        });
        menulist += "";
        $("#headNav").html(menulist);
        $('.navClick').eq(0).click();
    }

    //点击打开tab
    function onClickUrl(obj) {
        var url = $(obj).attr("dataurl");
        var tabTitle = $(obj).attr("datatext");
        $.ajax({
            url: "/PermissionManage/GetUserActionGrade",
            data: { url: url + "&check_Type=urltype" },
            dataType: "json",
            type: "post",
            async: true,
            //url: '/api/PermissionManage/GetUserActionGrade',
            //method: 'get',
            //contentType: 'application/json',
            //data: JSON.stringify(url),
            data: { url: url },

            success: function (d) {
                // var d = eval('(' + d + ')');
                if (d.Success) {
                    if (d.Data == 1) {
                        addTabs({ id: $(obj).attr('id'), title: $(obj).attr('datatext'), close: true, url: $(obj).attr('dataurl') }, obj);
                    } else if (d.Data == 2) {
                        //该功能需要身份验证才能操作
                        permissionIndex = layer.open({
                            title: '该功能需要身份验证才能操作',
                            type: 1,
                            area: ['400px', '200px'],
                            btn: ['确定', '取消'],
                            btnAlign: 'c',
                            content: $('#PermissionWin').html(),
                            yes: function (index) {
                                submitAuthenticationForUrl(tabTitle, url, obj);
                            },
                            cancel: function (index) {
                                layer.close(index);
                                layer.close(permissionIndex);
                                layer.close(processIndex);
                            }
                        });
                    } else if (d.Data == 3) {
                        //上级授权
                        permissionIndex = layer.open({
                            title: '该功能需要身份验证才能操作',
                            type: 1,
                            area: ['400px', '200px'],
                            btn: ['确定', '取消'],
                            btnAlign: 'c',
                            content: $('#SuperiorWin').html(),
                            yes: function (index) {
                                submitAuthoritiesHigherForUrl(tabTitle, url, obj);
                            },
                            cancel: function (index) {
                                layer.close(index);
                                layer.close(permissionIndex);
                                layer.close(processIndex);
                            }
                        });
                        //title = tabTitle + "需要上级授权才能浏览";
                        //$('#authorities_window').window({
                        //    draggable: true,
                        //    title: title
                        //});
                        //$('#authorities_window').window('open');
                        //$('#authorities_submit').linkbutton({
                        //    onClick: function () {
                        //        permissionIndex = layer.open({
                        //            title: '该功能需要身份验证才能操作',
                        //            type: 1,
                        //            area: ['400px', '200px'],
                        //            btn: ['确定', '取消'],
                        //            btnAlign: 'c',
                        //            content: $('#PermissionWin').html(),
                        //            yes: function (index) {
                        //                submitAuthoritiesHigherForUrl(tabTitle, url, addTaba);
                        //            },
                        //            cancel: function (index) {
                        //                layer.close(index);
                        //                layer.close(permissionIndex);
                        //                layer.close(processIndex);
                        //            }
                        //        });
                        //    }
                        //});
                    }
                } else {
                    if (d.Data == "-88") {
                        //用户没有权限
                        layer.alert('该用户没有该功能的权限！');
                    } else if (d.Data == "-99") {
                        if (isShowLogin)
                            return;
                        isShowLogin = true;
                        //用户没登录时的操作
                        $.messager.alert('提示', '用户登录超时！请重新登录！', 'error', function () {
                            loginShow();
                        });

                    } else {
                        // $.messager.alert('提示', '用户操作异常，请联系管理！');
                    }

                }
            }
        })
        //addTabs({ id: $(obj).attr('id'), title: $(obj).attr('datatext'), close: true, url: $(obj).attr('dataurl')}, obj);
    }

    //为url提交身份验证
    function submitAuthenticationForUrl(tabTitle, url, obj) {
        var pwd = $('#Password').val();

        if (pwd == "") {
            layer.alert('请输入密码');
            return;
        }
        var data = "filter_pwd=" + pwd + "&check_Type=urltype";
        $.ajax({
            url: url,
            data: data,
            dataType: 'json',
            type: 'post',
            async: false,
            success: function (d) {
                if (d.Success) {
                    layer.close(permissionIndex);
                    layer.close(processIndex);
                    url += "?UrlValidated=true";  //验证通过后，设置标识，过滤器不过滤
                    addTabs({ id: $(obj).attr('id'), title: $(obj).attr('datatext'), close: true, url: url }, obj);
                } else {
                    if (d.Data == "-55") {
                        layer.alert(d.Message);
                    }
                }
            },
            error: function () {
                layer.alert('输入密码出错，请重新输入！');
            }
        });
    }

    //点击菜单，为url提交上级授权
    function submitAuthoritiesHigherForUrl(tabTitle, url, obj) {
        var username = $('#Superior_name').val();
        var pwd = $('#Superior_pwd').val();
        if (pwd == "") {
            layer.alert('请输入密码');
            return;
        }

        //if (pwd == "") {
        //    $.messager.alert('提示', '请输入密码', 'info');
        //    return;
        //}

        //if (!ValidateInputType($.trim(username), "[^\a-\z\A-\Z0-9]")) {
        //    $.messager.alert('提示', '用户名不能空！仅限输入英文或数字的组合.', 'error');
        //    return;
        //}

        //if (!ValidateInputLength($.trim(username), 12)) {
        //    $.messager.alert('提示', '用户名长度不允许大于12位.', 'error');
        //    return;
        //}

        //if (!ValidateInputLength(pwd, 12)) {
        //    $.messager.alert('提示', '密码长度不能大于12位', 'error');
        //    return;
        //}

        var data = "filter_username=" + username + "&filter_pwd=" + pwd + "&check_Type=urltype";

        $.ajax({
            url: url,
            data: data,
            dataType: "json",
            type: "post",
            async: true,
            success: function (d) {
                if (d.Success) {
                    layer.close(permissionIndex);
                    layer.close(processIndex);
                    url += "?UrlValidated=true";  //验证通过后，设置标识，过滤器不过滤
                    addTabs({ id: $(obj).attr('id'), title: $(obj).attr('datatext'), close: true, url: url }, obj);

                } else {
                    if (d.Data == "-55") {
                        layer.alert(d.Message);
                    }
                }
            },
            error: function () {
                layer.alert('输入密码出错，请重新输入！');
            }
        });
    }