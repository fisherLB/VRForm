var addTabs = function (options,obj) {
    //可以在此处验证session
    //var rand = Math.random().toString();
    //var id = rand.substring(rand.indexOf('.') + 1);
    //debugger;
    //var url = window.location.protocol + '//' + window.location.host;
    options.url = options.url;
    id = "tab_" + options.id;
	var active_flag = false;
	if($("#" + id)){
		active_flag = $("#" + id).hasClass('active');
	}
    $(".active").removeClass("active");
    //如果TAB不存在，创建一个新的TAB
    if (!$("#" + id)[0]) {
        //固定TAB中IFRAME高度
        mainHeight = $(document.body).height();
        //创建新TAB的title
        title = '<li role="presentation" id="tab_' + id + '"><a href="#' + id + '" aria-controls="' + id + '" role="tab" data-toggle="tab"><i class="'+options.icon+'"></i>' + options.title;
        //是否允许关闭
        if (options.close) {
            title += ' <i class="glyphicon glyphicon-remove" tabclose="' + id + '"></i>';
        }
        title += '</a></li>';
        //是否指定TAB内容
        if (options.content) {
            content = '<div role="tabpanel" class="tab-pane" id="' + id + '">' + options.content + '</div>';
        } else {//没有内容，使用IFRAME打开链接
            content = '<div role="tabpanel" class="tab-pane" id="' + id + '"><iframe id="iframe_'+id+'" src="' + options.url + 
				'" width="100%" height="100%" onload="changeFrameHeight(this)" frameborder="no" border="0" marginwidth="0" marginheight="0" scrolling="yes" allowtransparency="yes"></iframe></div>';
        }
        //加入TABS
        $(".nav-tabs").append(title);
        $(".tab-content").append(content);
        //debugger;
        //计算当前已经存在的标签页的宽度如果大于1500（这个1500是定义的初始宽度），则加上当前的标签页的宽度
        var totalWidth=0;//这个12是首页关闭按钮的宽度
        $.each($(".nav-tabs li"),function(key, item){
               totalWidth+= $(item).width();
        });
        var tabsWidth = $(".nav-tabs").width();
        if(totalWidth>tabsWidth){
            var curWidth = $('#tab_'+id).width();
            $(".nav-tabs").width(tabsWidth + curWidth);
            var strLeft=$('.nav-tabs').css('left');
            var iLeft = parseInt(strLeft.replace('px', ''));
            iLeft=iLeft-curWidth;
            $(".nav-my-tab .middletab .nav-tabs").animate({left:iLeft + 'px'});
        }
    }else{
		if(active_flag){
			$("#iframe_" + id).attr('src', $("#iframe_" + id).attr('src'));
		}
	}
    //激活TAB
    $("#tab_" + id).addClass('active');
    $("#" + id).addClass("active");
        $('#headNav').find('li').removeClass('li_active');
        $(obj).parent('li').parent('ul').parent('li').addClass('li_active');
        $(obj).parent('li').addClass('li_active');
        addMenu();
        tabCloseEven();;
};
//右键事件
var curTabId = '';
function addMenu() {
    /*为选项卡绑定右键*/
    $(".nav-tabs li").bind('contextmenu', function (e) {
        curTabId = $(this).attr('id').substring($(this).attr('id').indexOf('_') + 1);
        $('#menu').show().css({
            left: e.pageX,
            top: e.pageY
        });
        $(".active").removeClass("active")
        $("#tab_" + curTabId).addClass('active');
        $("#" + curTabId).addClass('active');
        return false;
    });
    
    }

//绑定右键菜单事件
function tabCloseEven() {
    //刷新
    $('#mm-tabupdate').click(function () {
        document.getElementById('iframe_' + curTabId).contentWindow.location.reload(true);
        $('#menu').hide();
    });
    //关闭当前
    $('#mm-tabclose').click(function () {
        closeTab(curTabId);
        $('#menu').hide();
    });
    //全部关闭
    $('#mm-tabcloseall').click(function () {
        $('.nav-tabs li').each(function (i, n) {
            var id = $(this).attr('id').substring($(this).attr('id').indexOf('_') + 1);
            $("#tab_" + id).remove();
            $("#" + id).remove();
        });
        $('#menu').hide();
    });
    //关闭除当前之外的TAB
    $('#mm-tabcloseother').click(function () {
        $('.nav-tabs li').each(function (i, n) {
            var id = $(this).attr('id').substring($(this).attr('id').indexOf('_') + 1);
            if (id != curTabId) {
                $("#tab_" + id).remove();
                $("#" + id).remove();
            }
        });
        $("#tab_" + curTabId).addClass('active');
        $("#" + curTabId).addClass('active');
        $('#menu').hide();
    });
    //关闭当前右侧的TAB
    $('#mm-tabcloseright').click(function () {
        var nextall = $('.nav-tabs .active').nextAll();
        nextall.each(function (i, n) {
            var id = $(this).attr('id').substring($(this).attr('id').indexOf('_') + 1);
            $("#tab_" + id).remove();
            $("#" + id).remove();
        });
        $('#menu').hide();
        return false;
    });
    //关闭当前左侧的TAB
    $('#mm-tabcloseleft').click(function () {
        var prevall = $('.nav-tabs .active').prevAll();
        prevall.each(function (i, n) {
            var id = $(this).attr('id').substring($(this).attr('id').indexOf('_') + 1);
            $("#tab_" + id).remove();
            $("#" + id).remove();
        });
        $('#menu').hide();
        return false;
    });
}

var changeFrameHeight = function (that) {
    $(that).height(document.documentElement.clientHeight-125);
    $(that).parent(".tab-pane").height(document.documentElement.clientHeight-131);
}
var closeTab = function (id) {
    //如果关闭的是当前激活的TAB，激活他的前一个TAB
    if ($(".nav-tabs li.active").attr('id') == "tab_" + id) {
        $("#tab_" + id).next().addClass('active');
        $("#" + id).next().addClass('active');
        $("#tab_" + id).prev().addClass('active');
        $("#" + id).prev().addClass('active');
    }
    //关闭TAB
    $("#tab_" + id).remove();
    $("#" + id).remove();
};
$(function () {
    $("[addtabs]").click(function () {
        addTabs({ id: $(this).attr("id"), title: $(this).attr('title'), close: true });
    });

    $(".nav-tabs").on("click", "[tabclose]", function (e) {
        id = $(this).attr("tabclose");
        closeTab(id);
    });

    $('.nav-my-tab .middletab').width('100%');
    //固定左边菜单的高度
    $('#sidebar').height($(window).height() - 80);
    window.onresize = function () {
        var target = $(".tab-content .active iframe");
        changeFrameHeight(target);
    }

    //tab页向左向右移动
    $('.nav-my-tab .leftbackward').click(function(){
        var strLeft=$('.nav-my-tab .middletab .nav-tabs').css('left');
        var iLeft = parseInt(strLeft.replace('px', ''));
        if(iLeft>=0){
            return;
        }
        else{
            debugger;
            var totalWidth=0;
            var lis = $(".nav-tabs li");
            for(var i=0;i<lis.length;i++){
                var item = lis[i];
                totalWidth-= $(item).width();
                if(iLeft>totalWidth){
                    iLeft+=$(item).width();
                    break;
                }
            };
            if(iLeft>0){
                iLeft=0;
            }
            $(".nav-my-tab .middletab .nav-tabs").animate({left:iLeft + 'px'});
        }
    });
    $('.nav-my-tab .rightforward').click(function(){
            var strLeft=$('.nav-my-tab .middletab .nav-tabs').css('left');
            var iLeft = parseInt(strLeft.replace('px', ''));
            var totalWidth=0;
            $.each($(".nav-tabs li"),function(key, item){
                   totalWidth+= $(item).width();
            });
            var tabsWidth = $(".nav-my-tab .middletab").width();
            if(totalWidth>tabsWidth){
                debugger;
                if(totalWidth-tabsWidth<=Math.abs(iLeft)){
                    return;
                }
                var lis = $(".nav-tabs li");
                totalWidth=0;
                for(var i=0;i<lis.length;i++){
                    var item = lis[i];
                    totalWidth-= $(item).width();
                    if(iLeft>totalWidth){
                        iLeft-=$(item).width();
                        break;
                    }
                };
                $(".nav-my-tab .middletab .nav-tabs").animate({left:iLeft + 'px'});
            }

            
            
        });
});