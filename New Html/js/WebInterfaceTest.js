var checkState=false;
var outState="off";
var testJson={};
var newJson={
	"ACS-MARK":
	{
		"Input":
		{
			"X0":[3, 1, 2, ["急停", "加工按钮"]],
			"X1":[3, 2, 2, ["复位", "PC"]]	
		},

		"Output":
		{
			"Y0":[3, 3, 1, ["伺服上电", "直线电机"]],
			"Y1":[3, 2, 1, ["照明", "集尘"]]
		}
	},
	"ACS-AOI":
	{
		"Input":
		{
			"X0":[3, 1, 2, ["关机", "门锁"]],
			"X1":[3, 2, 2, ["上料有料", "磁簧"]]	
		},

		"Output":
		{
			"Y0":[3, 3, 1, ["光闸", "真空"]],
			"Y1":[3, 2, 1, ["翻转", "吸附"]]
		}
	}					
};
var isInit=false;
var firstJson=[];
var AJAXSuccess=false;
var AJAXFailFlag=0;
var AJAXQueryInterval;
var querytext="";
var controllerS="";
var selectIOType="";
var sendBeforeText="";
//TODO 阻塞Button, off掉button事件直到返回成功

$(document).ready(function() {
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
		// console.log("error");
		// $("div.AJAXFetching").hide();
		// $("div.AJAXFail").css('display', 'block');
		//test
		console.log("success");		
		testJson=newJson;	
		AJAXSuccesFunction();	
		$("div.beforeAJAX").hide().nextAll().show();	
		AJAXSuccess=true;	
		//test
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
	//AJAXQuery();
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
				var IONameFilter=$(filter).filter(":contains("+IOName+")");
				//this line need to change, contains not check start and end
				switch (IOType) {
					case "ONOFF":
						var isON=IONameFilter.nextAll("td.isOn").find("button");
						var x= (v=="ON")? (isON.text("ON").addClass('IOSwitchButtonON')
                        	 .removeClass('IOSwitchButtonOFF'))
                             :(isON.text("OFF").addClass('IOSwitchButtonOFF').removeClass('IOSwitchButtonON'));
						break;
					case "IOAlias":
					    var IOAlias=IONameFilter.nextAll("td.IOAlias");
					    IOAlias.text(v);
					    break;
					case "CheckStatus":
					    var isCheck=IONameFilter.nextAll("td.isChecked").find("div.checkMarkBox");
					    isCheck.removeClass().addClass('checkMarkBox')
                             .find('span').html('&#x2713');
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
	var firstSelector, lastSelector, cloneEle, cloneEleWrapper;
	var temp, validIOIndex, ONOFFIndex, checkIndex, IOAliasArr, 
	    firstSelectorChild, tempIndex, tempONOFF, tempIsChecked,
	    firstSelectorWrapper, lastSelectorWrapper;
	var  IOIndex, IOIsChecked, IOIsOn, IOAlias;
	var tempIndexW, tempONOFFW, tempIsCheckedW, tempIndexW2;
	var base;
	for (controllerName in testJson)
		{
			firstSelector=$("li.sideBarControllerList").first();
			lastSelector=$("li.sideBarControllerList").last()
			firstSelector.clone().css('display', 'block').insertAfter(lastSelector).
			   find('p.sideBarControllerName').text(controllerName);
			 switch (controllerName.split('-')[0]){
			 	case "ACS":base=8;break
			 	case "QPLC":base=16;break;
			 	default:alert('数据有错，建议检查');break;
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
			    	cloneEle.find("div.IOGroupName").text(IOGroupName);
			    	cloneEle.find('tr.IOTableHead').nextAll().remove();
			    	cloneEle.find('div.SingleWrapper').remove();
			    	//read cookie to find the default view point
			    	if (0==j)
			    	{
			    		firstJson=["."+controllerName, "."+IOType, "."+IOGroupName];
			    		controllerS=controllerName;
			    		selectIOType=IOType;
			    	}
			    	j++;

			    	temp=testJson[controllerName][IOType][IOGroupName];
			    	validIOIndex=temp[0].toString(2);
			    	ONOFFIndex=temp[1].toString(2);
			    	checkIndex=temp[2].toString(4);
			    	IOAliasArr=temp[3];

			    	var ol1=ONOFFIndex.length;
			    	var cl1=checkIndex.length;

			    	firstSelectorChild=$("tr.IOContentList").first();
			    	firstSelectorWrapper=$("div.SingleWrapper").first();   	

			    	var l=0,j=0;
			    	for(i=validIOIndex.length-1;i>=0;i--,ol1--,cl1--,j++)
			        {
			        	lastSelector=$("table.IOContentTableNo").last();
			        	lastSelectorWrapper=$("div.IOContentWrapper").last();
			        	if ('0'==validIOIndex[i])
			        	{
			        		cloneEle=firstSelectorWrapper.clone();
			        		CreateEmptyWrapper(base, j, cloneEle, 
			        			GetIOName(controllerName, IOGroupName, j));
			        		cloneEle.appendTo(lastSelectorWrapper);
			        		continue;
			        	}			        	
			        	cloneEle=firstSelectorChild.clone();
			        	cloneEleWrapper=firstSelectorWrapper.clone();			    	
			    	    tempIndex=cloneEle.find('td.IOName');
			    	    tempAlias=cloneEle.find('td.IOAlias');
				        tempONOFF=cloneEle.find('button.IOSwitchButtonBase');
				        tempIsChecked=cloneEle.find("div.checkMarkBox")	
				        tempIndexW=cloneEleWrapper.find('div.IOName');
				        tempONOFFW=cloneEleWrapper.find('div.isON');
				        tempIsCheckedW=cloneEleWrapper.find('div.isCheck');	
				        tempIndexW2=cloneEleWrapper.find('div.index');	        	

			        	IOIndex=i;
			        	IOAlias=IOAliasArr[l];
			        	IOIsOn= ol1>=1? (ONOFFIndex[ol1-1]=='1'):false;

			        	tempIndexW2.text(j.toString(base));
			        	tempIndex.text(GetIOName(controllerName, IOGroupName, j));
			        	tempIndexW.text(GetIOName(controllerName, IOGroupName, j));
			        	tempAlias.text(IOAlias);
			    		tempONOFF=IOIsOn ? (tempONOFF.addClass('IOSwitchButtonON').text("ON"))
			    		    :(tempONOFF.addClass('IOSwitchButtonOFF').text("OFF"));
			    		 tempONOFFW=IOIsOn ? (tempONOFFW.addClass('IOSwitchButtonON').text("1"))
			    		    :(tempONOFF.addClass('IOSwitchButtonOFF').text("0"));
			    		 tempIsChecked.removeClass().addClass('checkMarkBox')
			    		     .find('span').html('&#x2713');
			    		 tempIsCheckedW.removeClass().addClass('checkMarkBox');                         
			        	if (IOAlias.replace(/\s/g,"").length==0){
			        		tempIsCheckedW.addClass("NoAlias");
			        	}
                         if(cl1>=1)
                         {
                         	switch (checkIndex[cl1-1])
                         	{
                         		case "0":break;
                         		case "1":tempIsChecked.addClass('checkMarkBoxChecked');
                         		         tempIsCheckedW.addClass('checkMarkBoxChecked');
                         		   break;
                         		case "2":tempIsChecked.addClass('checkMarkBoxError')
                         		     .find('span').html('X');
                         		     tempIsCheckedW.addClass('checkMarkBoxError');
                         		     break;
                         		case "3":tempIsChecked.addClass('checkMarkBoxProblem')
	                         		 .find('span').html('?');
	                         		 tempIsCheckedW.addClass('checkMarkBoxProblem');
                         		     break;
                         		default:break;
                         	}
                         }
			    		cloneEle.appendTo(lastSelector);	
			    		cloneEleWrapper.appendTo(lastSelectorWrapper);
			    		l++;		    		
			        }

			        for (j;j<base;j++){
			        	cloneEle=firstSelectorWrapper.clone();
			        	CreateEmptyWrapper(base, j, cloneEle, 
			        		GetIOName(controllerName, IOGroupName, j));
			        	cloneEle.appendTo(lastSelectorWrapper);
			        }
			    }
			}
		}
}

function CreateEmptyWrapper(base, index, cloneEle, IOName){
	cloneEle.addClass('None');
	cloneEle.find('IOName').text(IOName);
	cloneEle.find('.isOn').text('-').addClass('None');
	cloneEle.find('isCheck').addClass('None');
	cloneEle.find('index').text(index.toString(base)).addClass('None');
}

function FillWrapper(IOname, ONOFF, CheckStatus, Index, base, cloneEle){
	var ONOFFEle, CheckEle;
	cloneEle.find('IOName').text(IOName);
	ONOFFEle = cloneEle.find('.isOn');
	ONOFF?(ONOFFEle.addClass('IOSwitchButtonON').text(1)):
	  (ONFFEle.addClass('IOSwitchButtonOFF').text(0));
	CheckEle = cloneEle.find('isCheck');
	switch (CheckStatus){
		case "0":CheckEle.removeClass().addClass('checkMarkBox').html('&#x2713');break;
		case "1":CheckEle
	}
	cloneEle.find('index').text(index.toString(base));
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
		selectIOType=$(this).text();
		var classFilter="."+controllerS+"."+selectIOType;		
		$("div.IOContentHead span").text(classFilter.split('.')[1]+"  "+classFilter.split('.')[2]);
		$("table.IOContentTableNo").hide().filter(classFilter).show();;
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