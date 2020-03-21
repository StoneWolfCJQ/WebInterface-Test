var testJson={};
var newJson={};
var isInit=false;
var firstJson=[];
var AJAXSuccess=false;
var querytext="";
var controllerS="";
var selectIOType="";
var sendBeforeText="";
//Log related
var maxLogNum=30;
var logDict={};//{c}
var filterP={"IO" : { "include" :[], "exclude":[]}};
var selectedLogControllerName="";
//storage
var filterStorage={};//{c:f}
var storageName="filter";
var scrollPosStorage={};
//{open:{controllerName:c, IOType:io},list:{c:{IOGroup:[iog],logOpen:true,scroll:{iot:height}}}
var scrollPosStorageName="scroll";
//ID
var IDMap=new Map();
var IDMapReverse=new Map();
var IDMapFirstChar='IDMap';
var lastModifiedSince="";

$(document).ready(function() {

	AJAXInit();
});

function AJAXInit(){
	$("div.beforeAJAX").nextAll().hide();	
	// ajax call
	var xhr = $.ajax({
		url: '/init/init.json',
		type: 'GET',
		dataType: 'json',
		timeout:5000,
	})
	.done(function(data) {
		console.log("success");		
		testJson=data;		
		$("div.beforeAJAX").hide().nextAll().show();	
		AJAXSuccess=true;	
		lastModifiedSince = xhr.getResponseHeader('Last-Modified');
		AJAXSuccesFunction();		
	})
	.fail(function() {
		console.log("error");
		$("div.AJAXFetching").hide();
		$("div.AJAXFail").css('display', 'block');
	})
	.always(function() {
		console.log("complete");
	});	
}

function AJAXSuccesFunction(){
	Init();
	SideBarRespond();
	ToggleIOTable();
	ToggleSideBar();
	ToggleONOFFButton();
	ToggleCheckBox();
	ToggleLogSwitchButton();
	ChangeLogTarget(selectedLogControllerName);
	TextAreaChange();
	FilterClearButtonEvent();
	LogClearButtonEvent();
	LogItemClickEvent();
	URLChangeEvent();
	//URLChangeEventAction(false);
	window.location.hash="";
	RestoreLastScroll();
	WindowScrollEvent();
	UnloadEvent();
	AJAXQuery();
}

var ajaxError=false;
function AJAXQuery(){
    sendBeforeText=querytext;
    if (querytext!="")
    {
    	var a=0;
    }
	var xhrs = $.ajax({
		url: '/query?'+querytext,
		beforeSend:function(xhr){
			xhr.setRequestHeader('If-Modified-Since', lastModifiedSince);
		},
		type: 'GET',
		dataType: 'json',
		timeout:2000,
	})
	.done(function(data) {
		newJson=data;
		if (sendBeforeText!="")
		{
			querytext="";
			EnableClickAJAX();
		}
		if (JSON.stringify(newJson)!="")
		{
			
		}
		lastModifiedSince=xhrs.getResponseHeader('Last-Modified');
		//if server send "refresh", require user to refresh the entire page
		HTMLChange();
		setTimeout(AJAXQuery, 100);		
	})
	.fail(function() {
		console.log("error");
		ajaxError=true;
		//tell user serve not response 
		//click button to retry
		$("div").hide();		
		$("div.AJAXFail").show().css('display', 'block').parent().show();
	})
	.always(function() {	
	});	
}

function HTMLChange()
{	
	//if there exits object that only is contained by one json,report error
	for (IOType in newJson)
	{
		for (controllerName in newJson[IOType])
		{			
			for (value in newJson[IOType][controllerName])
			{
				value=newJson[IOType][controllerName][value];
				var IOName=value.split('=')[0];
				var v=value.split('=')[1];
				var filter="table.IOContentTableNo."+controllerName+' td.IOName';
				var IONameFilter=$(filter).filter(function(){return $(this).text()==IOName;});
				var IOAlias=IONameFilter.nextAll("td.IOAlias");
				var aliasStr=IOAlias.text();
				switch (IOType) {
					case "ONOFF":
						var isON=IONameFilter.nextAll("td.isOn").find("button");
						if (isON.text()!=v){                    
	                        var logStr=IOName + " " + aliasStr +  " is $" + v;
	                        AddLogItem(controllerName, logStr);							
						}
						var x= (v=="ON")? (isON.text("ON").addClass('IOSwitchButtonON')
                        	 .removeClass('IOSwitchButtonOFF'))
                             :(isON.text("OFF").addClass('IOSwitchButtonOFF').removeClass('IOSwitchButtonON'));    
						break;
					case "IOAlias":					    
					    IOAlias.text(v);
					    break;
					case "CheckStatus":
					    var isCheck=IONameFilter.nextAll("td.isChecked").find("div.checkMarkBox");
					    if (!isCheck.hasClass(v+'LogMark')){
	                        var logStr=IOName + " " + aliasStr +  " is $" + v;
	                        AddLogItem(controllerName, logStr);					    	
					    }
					    isCheck.removeClass().addClass('checkMarkBox').find('span').html('&#x2713');
                         switch (v)
                         	{
                         		case "Uncheck": isCheck.addClass('UncheckLogMark');break;
                         		case "Checked":isCheck.addClass('checkMarkBoxChecked').addClass('CheckedLogMark');break;
                         		case "Error":isCheck.addClass('checkMarkBoxError').addClass('ErrorLogMark')
                         		     .find('span').html('X');
                         		     break;
                         		case "Problem":isCheck.addClass('checkMarkBoxProblem').addClass('ProblemLogMark')
	                         		 .find('span').html('?');
                         		     break;
                         		default:break;
                         	}
                         break;
					default:
						// statements_def
						break;
				}
			}
		}
	}
}

function CheckJsonInit(){
	for (i in testJson)
	{
		if ((null==i)||(undefined==i))
		{
			AJAXFail
		}
	}
}

function Init(){
	GetStorage();
	TableContentInit();
	TableDisplayInit();
	SetStorage();
	isInit=true;
}

function GetStorage(){
	if (typeof(Storage)!==undefined){
		filterStorage=JSON.parse(localStorage.getItem(storageName));
		scrollPosStorage=JSON.parse(localStorage.getItem(scrollPosStorageName));
	}
	else {
		alert("浏览器不支持本地存储");
	}
}

function SetStorage(){
	if (typeof(Storage)===undefined){
		return;
	}
	filterStorage={};
	for (var cName in logDict){
		filterStorage[cName] = logDict[cName]["filter"];
	}
	localStorage.setItem(storageName, JSON.stringify(filterStorage));
	SetScrollStorage();
}

function SetScrollStorage(){
	if (typeof(Storage)===undefined){
		return;
	}
	localStorage.setItem(scrollPosStorageName, JSON.stringify(scrollPosStorage));
}

function RestoreLastScroll(){
	var s=scrollPosStorage;
	for (var cName in s["list"]){
		for (var ign in s["list"][cName]["IOGroup"]){
			var o=FindIOTableHeadByControllerNameAndIOGroup(cName, s["list"][cName]["IOGroup"][ign]);
			if (o){				
				ToggleIOTableAction(o, 'click');
			}
		}
	}
	var ocn=s["open"]["controllerName"];
	var oi=s["open"]["IOType"];	
	if (ocn&&oi){	    
		controllerS=ocn;
		selectedLogControllerName=ocn;
		selectIOType=oi;			
		var o2=FindSideBarFromControllerNameAndIOType(ocn, oi);
		SideBarRespondAction(o2);
		$(window).animate({scrollTop: s["list"][ocn]["scroll"][oi]});		
		LogWindowsRestore(ocn);
	}
}

function LogWindowsRestore(controllerName){
	if (scrollPosStorage["list"][controllerName]["logOpen"]){
		ToggleLogSwitchButtonAction('on');
	}
	else{
		ToggleLogSwitchButtonAction('off');
	}
}

function TableContentInit(){
	var j=0;
	var firstSelector, lastSelector, cloneEle;
	var temp, validIOIndex, ONOFFIndex, checkIndex, IOAliasArr, 
	    firstSelectorChild, tempIndex, tempONOFF, tempIsChecked;
	var  IOIndex, IOIsChecked, IOIsOn, IOAlias;
	var first=true;
	var IDMapNum=0;
	var IDMapStr="";
	var s=scrollPosStorage;
	var hasOpenCName=false;
	var k={open:{controllerName:"", IOType:""}, list:{}};
	if (!s){
		s=k;
	}

	for (controllerName in testJson)
		{
			firstSelector=$("li.sideBarControllerList").first();
			lastSelector=$("li.sideBarControllerList").last()
			firstSelector.clone().css('display', 'block').insertAfter(lastSelector).
			   find('p.sideBarControllerName').text(controllerName);
			var logContent={"log":["Initializing"], "filter":""};
		    if (filterStorage&&(filterStorage[controllerName])){
		    	logContent["filter"]=filterStorage[controllerName];
		    }
			logDict[controllerName]=logContent;			

			k["list"][controllerName]={IOGroup:[], logOpen:false, scroll:{}}
			if (!s["list"][controllerName]){
				s["list"][controllerName]=k["list"][controllerName];
			}
			k["list"][controllerName]["logOpen"]=s["list"][controllerName]["logOpen"];


			for (IOType in testJson[controllerName])
			{	
				k["list"][controllerName]["scroll"][IOType]=0;			
				if (s["list"][controllerName]["scroll"][IOType])
				{
					k["list"][controllerName]["scroll"][IOType]=
						 s["list"][controllerName]["scroll"][IOType]
				}

				if ((s["open"]["controllerName"]==controllerName)&&(s["open"]["IOType"]==IOType)){
					hasOpenCName=true;
				}
				
			    for (IOGroupName in testJson[controllerName][IOType])
			    {
			    	if (s["list"][controllerName]["IOGroup"].indexOf(IOGroupName)!=-1){
				    	k["list"][controllerName]["IOGroup"].push(IOGroupName);			    		
			    	}
			    	if (first){
						selectedLogControllerName=controllerName;
						k["open"]["controllerName"]=controllerName;	
						k["open"]["IOType"]=IOType;
						controllerS=controllerName;
						selectIOType=IOType;
						first=false;	
					}
			    	firstSelector=$("table.IOContentTableNo").first();
					lastSelector=$("table.IOContentTableNo").last();

					cloneEle=firstSelector.clone();
					cloneEle.addClass(controllerName).addClass(IOType).
					    addClass(IOGroupName).insertAfter(lastSelector);
			    	cloneEle.find("th.IOGroupName").text(IOGroupName);
			    	cloneEle.find('tr.IOTableHead').nextAll().remove();
			    	//read cookie to find the default view point
			    	if (0==j)
			    	{
			    		firstJson=["."+controllerName, "."+IOType, "."+IOGroupName];
			    		controllerS=controllerName;
			    		selectIOType=IOType;
			    	}
			    	j++;

			    	temp=testJson[controllerName][IOType][IOGroupName];
			    	validIOIndex=ConvertSignedIntToString(temp[0],2);
			    	ONOFFIndex=ConvertSignedIntToString(temp[1],2);
			    	checkIndex=ConvertSignedIntToString(temp[2],4);
			    	IOAliasArr=temp[3];

			    	var ol1=ONOFFIndex.length;
			    	var cl1=checkIndex.length;

			    	firstSelectorChild=$("tr.IOContentList").first();			    	

			    	var l=0,j=0;
			    	for(i=validIOIndex.length-1;i>=0;i--,ol1--,cl1--,j++)
			        {
			        	if ('0'==validIOIndex[i])
			        		continue;
			        	lastSelector=$("table.IOContentTableNo").last();
			        	cloneEle=firstSelectorChild.clone(); 	
			    	    tempIndex=cloneEle.find('td.IOName');
			    	    tempAlias=cloneEle.find('td.IOAlias');
				        tempONOFF=cloneEle.find('button.IOSwitchButtonBase');
				        tempIsChecked=cloneEle.find("div.checkMarkBox")			        	

			        	IOIndex=i;
			        	IOAlias=IOAliasArr[l];
			        	IOIsOn= ol1>=1? (ONOFFIndex[ol1-1]=='1'):false;

			        	tempIndex.text(GetIOName(controllerName, IOGroupName, j));	
			        	IDMapStr=IDMapFirstChar+IDMapNum;
			        	IDMapNum++;
			        	IDMap.set(controllerName+'-' + tempIndex.text(), IDMapStr);	 
			        	IDMapReverse.set(IDMapStr, [controllerName, tempIndex.text()]);       	
			        	cloneEle.attr('id', IDMapStr);			   
			        	tempAlias.text(IOAlias);
			    		tempONOFF=IOIsOn ? (tempONOFF.text("ON").addClass('IOSwitchButtonON').text("ON"))
			    		    :(tempONOFF.text("ON").addClass('IOSwitchButtonOFF').text("OFF"));
			    		 tempIsChecked.removeClass().addClass('checkMarkBox').find('span').html('&#x2713');
                         if(cl1>=1)
                         {
                         	switch (checkIndex[cl1-1])
                         	{
                         		case "0":tempIsChecked.addClass('UncheckLogMark');break;
                         		case "1":tempIsChecked.addClass('checkMarkBoxChecked').addClass('CheckedLogMark');break;
                         		case "2":tempIsChecked.addClass('checkMarkBoxError').addClass('ErrorLogMark')
                         		     .find('span').html('X');
                         		     break;
                         		case "3":tempIsChecked.addClass('checkMarkBoxProblem').addClass('ProblemLogMark')
	                         		 .find('span').html('?');
                         		     break;
                         		default:break;
                         	}
                         }
			    		cloneEle.appendTo(lastSelector);
			    		l++;		    		
			        }
			    }
			}
			AddLogItem(controllerName, "Success");
		}
		if (hasOpenCName){
			k["open"]=s["open"];
		}
		scrollPosStorage=k;
}

function ConvertSignedIntToString(input, base){
	if (input<0){
		return (input>>>0).toString(base);
	}
	else{
		return input.toString(base);
	}
}

function GetIOName(cName, IOGroupName, index){
	var cType=cName.split('-')[0];
	switch (cType) {
		case "ACS":
		case "雷赛":
			return IOGroupName+"."+index;
		case "QPLC":
			return IOGroupName.split('-')[0].substring(0, IOGroupName.split('-')[0].length-1)+index.toString(16).toUpperCase();
		default:
			alert("数据有错，建议检查");
			break;
	}
}

function TableDisplayInit(){
	var defaultTableDisplay=[];
	// defaultTableDisplay=something
	defaultTableDisplay=firstJson[0]+firstJson[1];
	$("table.IOContentTableNo").filter(defaultTableDisplay).css('display', 'table');
	$("div.IOContentHead span").text(firstJson[0].replace('.','')+"  "+firstJson[1].replace('.',''));
}

function ToggleSideBar(){
	$("div.sideBarControllerActionContainer").on('click',function(event) {
		event.preventDefault();
		/* Act on the event */
		var nextUL=$(this).next("ul");
		nextUL.toggleClass('sideBarControllerIOListClick');
		if (nextUL.hasClass('sideBarControllerIOListClick'))
		{
			$(this).children('p.downArrow').html("&#x25B2");
		}
		else
		{
			$(this).children('p.downArrow').html("&#x25BC");
		}
	});
}

function ToggleONOFFButton(){
	$("button.IOSwitchButtonBase").on('click', function(event) {
		
		event.preventDefault();
		/* Act on the event */
		
		 if (selectIOType=="Output")
		 {
		 	$(this).toggleClass('IOSwitchButtonON').toggleClass('IOSwitchButtonOFF').
		      css('animation-play-state', 'running'); 
			 $(this).on('animationend', function(){$(this).css('animation-play-state', 'paused');});
			 var IOName=$(this).parent().prevAll("td.IOName").text();
			 var out=$(this).text()=="ON"?"OFF":"ON";
			 querytext="Controller="+controllerS+"&"+IOName+"="+out;
			 DisableClickAJAX();
		 }
		
	});
}

function ToggleCheckBox(){
	$("div.checkMarkBox").on('dblclick', function(event) {
		event.preventDefault();
		/* Act on the event */
		var IOName=$(this).parent().prevAll("td.IOName").text();
		var out, result;
		out="Checked";
		if ($(this).hasClass('checkMarkBoxChecked'))
		{
			out="Error";
		}
		else if ($(this).hasClass('checkMarkBoxError'))
		{
			out="Problem";
		}
		else if ($(this).hasClass('checkMarkBoxProblem'))
		{
			out="Uncheck";
		}
		querytext="Controller="+controllerS+"&"+IOName+"="+out;
		DisableClickAJAX();
	});
}

function SideBarRespond(){
	$("li").filter("[class^=sideBarMenu]").on('click', function(event) {
		event.preventDefault();
		SideBarRespondAction($(this));
	});
}

function SideBarRespondAction(selector){
	var tcontroller=controllerS
	controllerS=selector.parent().prev().children("p.sideBarControllerName").text();
	selectedLogControllerName=controllerS;
	selectIOType=selector.text();	
	var s=scrollPosStorage;
	s["open"]["controllerName"]=controllerS;
	s["open"]["IOType"]=selectIOType;
	var classFilter="."+controllerS+"."+selectIOType;		
	$("div.IOContentHead span").text(classFilter.split('.')[1]+"  "+classFilter.split('.')[2]);
	$("table.IOContentTableNo").hide().filter(classFilter).show();
	if (tcontroller!=controllerS){
		ChangeLogTarget(controllerS);	
	}
	$(window).scrollTop(s["list"][controllerS]["scroll"][selectIOType]);
	SetScrollStorage();
}

function ToggleIOTable(){
	$("tr.IOTableHead").on('click', function(event) {
		event.preventDefault();
		ToggleIOTableAction($(this));
	});
}

function ToggleIOTableAction(selector, actionNo='none'){	
	var nextTr=selector.nextAll("tr.IOContentList");
	var IOGroupName=selector.find('.IOGroupName').text();
	/* Act on the event */
	nextTr.find('button').css('animation-play-state', 'paused');
	switch (actionNo) {
		case 'none':
			nextTr.toggleClass("IOContentListClick");
			break;
	    case 'click':
		    if (!nextTr.first().hasClass("IOContentListClick")){
		    	nextTr.toggleClass("IOContentListClick");
		    }
		    break;
		default:
			// statements_def
			break;
	}
	var si=scrollPosStorage["list"][controllerS]["IOGroup"].indexOf(IOGroupName);
	if (nextTr.first().hasClass("IOContentListClick"))
	{			
		if (si==-1){
			scrollPosStorage["list"][controllerS]["IOGroup"].push(IOGroupName)
		}
		selector.find("th.IODownArrow").html("&#x25B2");
	}
	else
	{		
	    if (si!=-1){	
	    	scrollPosStorage["list"][controllerS]["IOGroup"].splice(si, 1);
	    }
		selector.find("th.IODownArrow").html("&#x25BC");
	}
	SetScrollStorage();
}

function URLChangeEvent(){
	$(window).on('hashchange',  function(event) {
		event.preventDefault();
		URLChangeEventAction();
	});
}

function URLChangeEventAction(fromHistory=true){
	var idStr=window.location.href;
	if (idStr.indexOf('#')!=-1){
		idStr=idStr.substring(idStr.indexOf('#')+1);
		if (IDMapReverse.get(idStr)){
			ScrollToIO1(IDMapReverse.get(idStr)[0], IDMapReverse.get(idStr)[1], fromHistory);				
		}
		else{
			var s=window.location.href;
			window.location.hash = "";
		}
	}
}

function WindowScrollEvent(){
	$(window).on('scroll',  function(event) {
		event.preventDefault();		
		if (!ajaxError){			
	        scrollPosStorage["list"][controllerS]["scroll"][selectIOType]=$(window).scrollTop();
	        SetScrollStorage();
		}
	});
}

function UnloadEvent(){
	$(window).on('unload',  function(event) {
		event.preventDefault();
		if (!ajaxError){			
			scrollPosStorage["list"][controllerS]["scroll"][selectIOType]=$(window).scrollTop();
			SetScrollStorage();
		}
	});
}

function ScrollToIO1(controllerName, IOName, fromHistory = false){	
	var IOtableHead = $('#'+IDMap.get(controllerName+'-'+IOName)).first().prevAll('tr.IOTableHead');
	var tempc=IOtableHead.parents("table").attr('class');
	var IOType = tempc.split(' ')[2];
	var sideBarController=FindSideBarFromControllerNameAndIOType(controllerName, IOType);
	if (IOtableHead&&sideBarController){		
		SideBarRespondAction(sideBarController);//, true);
		ToggleIOTableAction(IOtableHead, 'click');
		var s=window.location.href;
		if (fromHistory || !fromHistory){
			if (s.indexOf('#')!=-1){			
				s=s.substring(0, s.indexOf('#')+1) +IDMap.get(controllerName+'-'+IOName);
			}
			else{
				s=s+'#'+IDMap.get(controllerName+'-'+IOName);
			}
			$(window).off('hashchange');
			$(window).off('scroll');
			window.location=s;	
			URLChangeEvent();	
			WindowScrollEvent();
		}
	}
}

function FindIOTableHeadByControllerNameAndIOGroup(controllerName, IOGroup){
	return $('table.'+controllerName+'.'+IOGroup).find('tr.IOTableHead');
}

function FindSideBarFromControllerNameAndIOType(controllerName, IOType){
	return $('div.sideBarControllerActionContainer').filter(function(index) {
			return $(this).children('.sideBarControllerName').text()==controllerName;
			}).next().find('li.sideBarMenu'+IOType);
}

function DisableClickAJAX(){
	$("button.IOSwitchButtonBase").off('click');
	$("div.checkMarkBox").off('dblclick');
}

function EnableClickAJAX(){
	ToggleONOFFButton();	
	ToggleCheckBox();	
}

function ToggleLogSwitchButton(){
	$("div.IOLog .switchButton").on('click', function(event) {
		event.preventDefault();
		ToggleLogSwitchButtonAction();
		/* Act on the event */
	});
}

function ToggleLogSwitchButtonAction(action = 'none'){
	switch (action) {
		case 'on':
		case 'off':
			$("div.IOLog").removeClass('on').removeClass('off').addClass(action);
			break;
		default:
			$("div.IOLog").toggleClass('on').toggleClass('off');
			break;
	}	
	scrollPosStorage['list'][controllerS]['logOpen']=$("div.IOLog").hasClass('on')?true:false;
	SetScrollStorage();
}

function TextAreaChange(){
	var t=$("div.filter .content .filterText");
	t.change(function(event) {
		/* Act on the event */
		TextAreaChangeEvent();
	});		
}

function TextAreaChangeEvent(){
	var t=$("div.filter .content .filterText");
	logDict[selectedLogControllerName]["filter"]=t.val();
	ParseFilter(t.val());	
	RewriteHtmlLog(selectedLogControllerName);
	SetStorage();
}

function HtmlLogWrite(s){
	if (HtmlLogFilterBeforeWrite(s)){	
	    var c=	$("div.log .content");
		c.append("<p class='item'>>"+s+"</p>");
		LogItemClickEvent();
		var sh=c[0].scrollHeight;
		var st=c[0].scrollTop;
		var ch=c[0].clientHeight;
		var cch=c.children(':last-child').height();		
		if (sh-st-ch<=1.5*cch){		
			c.animate({scrollTop: 9999});
		}
	}
}

function HtmlLogFilterBeforeWrite(s){
	var ex=filterP["IO"]["exclude"];
	var ix=filterP["IO"]["include"];
	var result=true;
	for (var ixsi in ix){
		var ixs=ix[ixsi];
		result=false;
		if ((ixs!="")&&(ixs!=" ")){
			if (s.indexOf(ixs, 0)!=-1){
				result=true;
				break;
			}
		}
	}

	for (var exsi in ex){
		var exs=ex[exsi];
		if ((exs!="")&&(exs!=" ")){
			if (s.indexOf(exs, 0)!=-1){
				result=false;
				break;
			}
		}
	}
	return result;
}

function HtmlLogClear(){
	$("div.log .content").text("");
}

function HtmlFilterWrite(s){
	$("div.filter .content textarea").val(s);
}

function ParseFilter(s){
	if (s==null){
		return;
	}
	var ss = s.split(/[\s \r\n]+/);
	var ex=[];
	var ix=[];
	if (ss.length>0){
		for (var csi in ss){
			if (ss[csi]==="")	{
				break;
			}
            var cs=ss[csi];
			if (cs.startsWith('-', 0)){
			    cs=cs.substring(1); 
			    if (ex.indexOf(cs, 0)==-1){			    	
					ex.push(cs);
			    }
			}
			else {
				if (ix.indexOf(cs, 0)==-1){
					ix.push(cs);
				}
			}
		}
	}
	filterP["IO"]["include"]=ix;
	filterP["IO"]["exclude"]=ex;
}

function ChangeLogTarget(controllerName){
	var ch=$("div.IOLog .controller");
	HtmlFilterWrite(logDict[controllerName]["filter"]);
    TextAreaChangeEvent();
	if (ch.text()!=controllerName){		
		RewriteHtmlLog(controllerName);
	}	
	LogWindowsRestore(controllerS);
}

function RewriteHtmlLog(controllerName){
	    HtmlLogClear();
		$("div.IOLog .controller").text(controllerName);
		for (s in logDict[controllerName]["log"]){
			HtmlLogWrite(logDict[controllerName]["log"][s]);
		}
}

function AddLogItem(controllerName, logStr){
	var logSel=logDict[controllerName]["log"];
	if (logSel[logSel.length-1]==logStr){
		return
	}
	logSel.push(logStr);
	if (logSel.length>maxLogNum){
		logSel.splice(0, 1);
	}
	if (controllerName==selectedLogControllerName){
		HtmlLogWrite(logStr)
		var c=$("div.log .content").children();
		if (c.length>maxLogNum){
			c[0].remove();
		}
	}
}

function FilterClearButtonEvent(){
	$("div.IOLog .filterClear").on('click',  function(event) {
		event.preventDefault();
		filterP["IO"]["include"]=[];
		filterP["IO"]["exclude"]=[];
		logDict[selectedLogControllerName]["filter"]="";
		HtmlFilterWrite(logDict[selectedLogControllerName]["filter"]);
		RewriteHtmlLog(selectedLogControllerName);		
	});
}

function LogClearButtonEvent(){
	$("div.IOLog .logClear").on('click',  function(event) {
		event.preventDefault();
		logDict[selectedLogControllerName]["log"]=[];
		HtmlLogClear();
	});
}

function LogItemClickEvent(){
	$("div.IOLog .mainBody .log .content .item").off('click');
	$("div.IOLog .mainBody .log .content .item").on('click',  function(event) {
		event.preventDefault();
		var tIOName=$(this).text().split(' ')[0].substring(1);
		if ((tIOName!="Initializing")&&(tIOName!="Success")){
			ScrollToIO1(selectedLogControllerName, tIOName);
		}
	});
}