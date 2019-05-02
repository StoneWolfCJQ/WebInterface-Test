var checkState=false;
var outState="off";
var testJson={};
var newJson={};
var isInit=false;
var firstJson=[];
var AJAXSuccess=false;
var AJAXFailFlag=0;
var AJAXQueryInterval;
var querytext="";
var controllerS="";
var selectIOType="";
var sendBeforeText="";
//Log related
var maxLogNum=30;
var logDict={};
var filterP={"IO" : { "include" :[], "exclude":[]}};
var selectedLogControllerName="";
//TODO 阻塞Button, off掉button事件直到返回成功
$(document).ready(function() {
	// AJAXSuccesFunction();
	AJAXInit();
});

function AJAXInit(){
	$("div.beforeAJAX").nextAll().hide();	
	// ajax call
	$.ajax({
		url: '/init/init.json',
		type: 'GET',
		dataType: 'json',
		timeout:10000,
	})
	.done(function(data) {
		console.log("success");		
		testJson=data;	
		AJAXSuccesFunction();	
		$("div.beforeAJAX").hide().nextAll().show();	
		AJAXSuccess=true;			
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
	ToggleSwitchButton();
	ChangeLogTarget(selectedLogControllerName);
	TextAreaChange();
	FilterClearButtonEvent();
	LogClearButtonEvent()
	AJAXQuery();
}

function AJAXQuery(){
    sendBeforeText=querytext;
	$.ajax({
		url: '/query?'+querytext,
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
		//if server send "refresh", require user to refresh the entire page
		HTMLChange();
		setTimeout(AJAXQuery, 100);		
	})
	.fail(function() {
		console.log("error");
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
						var x= (v=="ON")? (isON.text("ON").addClass('IOSwitchButtonON')
                        	 .removeClass('IOSwitchButtonOFF'))
                             :(isON.text("OFF").addClass('IOSwitchButtonOFF').removeClass('IOSwitchButtonON'));                        
                        var logStr=IOName + " " + aliasStr +  " is $" + v;
                        AddLogItem(controllerName, logStr);
						break;
					case "IOAlias":					    
					    IOAlias.text(v);
					    break;
					case "CheckStatus":
					    var isCheck=IONameFilter.nextAll("td.isChecked").find("div.checkMarkBox");
					    isCheck.removeClass().addClass('checkMarkBox')
                             .find('span').html('&#x2713');
                        var logStr=IOName + " " + aliasStr +  " is $" + v;
                        AddLogItem(controllerName, logStr);
                         switch (v)
                         	{
                         		case "Uncheck":break;
                         		case "Checked":isCheck.addClass('checkMarkBoxChecked');break;
                         		case "Error":isCheck.addClass('checkMarkBoxError')
                         		     .find('span').html('X');
                         		     break;
                         		case "Problem":isCheck.addClass('checkMarkBoxProblem')
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
	TableContentInit();
	TableDisplayInit();
	isInit=true;
}

function TableContentInit(){
	var j=0;
	var firstSelector, lastSelector, cloneEle;
	var temp, validIOIndex, ONOFFIndex, checkIndex, IOAliasArr, 
	    firstSelectorChild, tempIndex, tempONOFF, tempIsChecked;
	var  IOIndex, IOIsChecked, IOIsOn, IOAlias;
	var first=true;
	for (controllerName in testJson)
		{
			firstSelector=$("li.sideBarControllerList").first();
			lastSelector=$("li.sideBarControllerList").last()
			firstSelector.clone().css('display', 'block').insertAfter(lastSelector).
			   find('p.sideBarControllerName').text(controllerName);
			var logContent={"log":["Initializing"], "filter":""};
			logDict[controllerName]=logContent;	
			if (first){
				selectedLogControllerName=controllerName;	
				first=false;	
			}			

			for (IOType in testJson[controllerName])
			{				
			    for (IOGroupName in testJson[controllerName][IOType])
			    {
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
			        	tempAlias.text(IOAlias);
			    		tempONOFF=IOIsOn ? (tempONOFF.text("ON").addClass('IOSwitchButtonON').text("ON"))
			    		    :(tempONOFF.text("ON").addClass('IOSwitchButtonOFF').text("OFF"));
			    		 tempIsChecked.removeClass().addClass('checkMarkBox')
			    		     .find('span').html('&#x2713');
                         if(cl1>=1)
                         {
                         	switch (checkIndex[cl1-1])
                         	{
                         		case "0":break;
                         		case "1":tempIsChecked.addClass('checkMarkBoxChecked');break;
                         		case "2":tempIsChecked.addClass('checkMarkBoxError')
                         		     .find('span').html('X');
                         		     break;
                         		case "3":tempIsChecked.addClass('checkMarkBoxProblem')
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

function ToggleIOTable(){
	$("tr.IOTableHead").on('click', function(event) {
		event.preventDefault();
		var nextTr=$(this).nextAll("tr.IOContentList");
		/* Act on the event */
		nextTr.find('button').css('animation-play-state', 'paused');
		nextTr.toggleClass("IOContentListClick");
		if (nextTr.hasClass("IOContentListClick"))
		{			
			$(this).find("th.IODownArrow").html("&#x25B2");
		}
		else
		{			
			$(this).find("th.IODownArrow").html("&#x25BC");
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
		/* Act on the event */
		controllerS=$(this).parent().prev().children("p.sideBarControllerName").text();
		selectedLogControllerName=controllerS;
		selectIOType=$(this).text();
		var classFilter="."+controllerS+"."+selectIOType;		
		$("div.IOContentHead span").text(classFilter.split('.')[1]+"  "+classFilter.split('.')[2]);
		$("table.IOContentTableNo").hide().filter(classFilter).show();
		ChangeLogTarget(controllerS);
	});
}

function DisableClickAJAX(){
	$("button.IOSwitchButtonBase").off('click');
	$("div.checkMarkBox").off('dblclick');
}

function EnableClickAJAX(){
	ToggleONOFFButton();	
	ToggleCheckBox();	
}

function ToggleSwitchButton(){
	$("div.switchButton").on('click', function(event) {
		event.preventDefault();
		$("div.IOLog").toggleClass('on').toggleClass('off');;
		/* Act on the event */
	});
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
}

function HtmlLogWrite(s){
	if (HtmlLogFilterBeforeWrite(s)){		
		$("div.log .content").append("<p class='item'>>"+s+"</p>");
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
		/* Act on the event */
	});
}

function LogClearButtonEvent(){
	$("div.IOLog .logClear").on('click',  function(event) {
		event.preventDefault();
		logDict[selectedLogControllerName]["log"]=[];
		HtmlLogClear();
		/* Act on the event */
	});
}