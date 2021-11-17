if(hpdq==null||hpdq==undefined)var hpdq={};
if(hpdq.tab==null||hpdq.tab==undefined)hpdq.tab={};
hpdq.tab = {
  id: "mytabs",
  Ctx: "mytab_ctx",
  pre:'m_',
  _tabs:null,
  curIdCtx:'',
  showType:1,
  getEvent : function(event){
    var evtSource;
    if (document.all){ // If IE
      evtSource = event.srcElement;
    } else{ // For Firefox//
      evtSource = event.target;
    }
    return evtSource;
  },
PinOnOff : function (obj) {
   var ch=$(obj).children(1);
    if ($(ch)) {
        if ($(ch).attr('status') == 0) {
          $(ch).attr ('class' , 'glyphicon glyphicon-pushpin');
          $(ch).attr('status', 1);
        } else {
          $(ch).attr ('class' , 'fas fa-thumbtack');
          $(ch).attr('status', 0);
        }
        this.showType = $(ch).attr('status');
    }
},
FindTab : function (_pk) {
 var cnt= $('#'+this.id+' li').size();
   var obj= document.getElementById(this.id);
	for(var t=1; t< cnt;t++){
   //while (obj.children(t).prop("tagName") != "LI")
  //  idTab = obj.children(t).children();
      var idTab = obj.children[t];  
	    if (idTab) {       
        if(this.pre+_pk==idTab.getAttribute('id-content'))
					return t;
				}		
	}
    return -1;
},
  NewWindow: function (url, Title, menu_cd, menu_id,_pk){
    if (this.showType == 1) {	
      idx = this.FindTab(_pk);
      if (idx != -1) {        
  	$('#'+this.id+' a:eq('+idx+')').tab('show');
    $('#'+this.id).bootstrapDynamicTabs();
        var currForm = $('#'+this.pre+ _pk );   
        if(currForm){
          var frm = currForm.children(1);
	        if(frm){
	        	if(frm[0]){
              if (confirm("Do you want to reload this form ?")) {
                url=frm[0].src;
                frm[0].src = url;                
              }            
	        	}
	        }
        }
      }else {
        this.AddTab(url, Title, menu_id, menu_cd,_pk);
      }
   }else {
          this.AddTab(url, Title, menu_id, menu_cd,_pk);
  }
  },
  AddTab:function(url, Title, menu_id, menu_cd,_pk){
  //var nextTab = $('#mytabs li').size();  
      var hpdqh = (screen.height - 265)+'px';
      var content = '<div class="tab-pane" style="height:'+ hpdqh +'" id="'+this.pre + _pk  + '">';
    content += '<iframe id="frmContent" src="'+((window['ctx']=== '/') ? url : url.replace('..', '')) + '"  style="padding:0 0 0 0; overflow:hidden; width:100%; height:100%;" frameborder=0  height="100%" width="100%" objid="'+menu_id+'" version="0" lang="0">	</iframe>'
    content +='</div>';
     $('#'+this.id).append('<li id-content="'+this.pre+_pk+'" title="'+url+'"><a  href="#'+this.pre + _pk + '" data-toggle="pill" data-tip="tooltip"  title="'+Title+'" >'+ menu_cd + ' ' + Title+'</a><i class="fa fa-times icon" onclick="hpdq.tab.RemoveTab(this)"></i></li>');
    $('.tab-content').append(content);
  // make the new tab active
  	$('#'+this.id+' a:last').tab('show');
    $('#'+this.id).bootstrapDynamicTabs();
    this.curIdCtx=this.pre+_pk;
  //nextTab= nextTab+1;
  $('[data-tip="tooltip"]').tooltip();  
 },
 RemoveTab:function(obj){
        var tabContentID = $(obj).parent().attr('id-content');
         $(obj).parents('li').remove();
         $('#'+tabContentID).remove(); 
         //display first tab :nth-child(2)         
         var tabFirst = $('#'+ this.id +' a:first');
         tabFirst.tab('show');
       
 
 },
 RemoveTabCurr: function(){
  $("[id-content="+this.curIdCtx+"]").remove();
  $('#'+this.curIdCtx).remove(); 
  //display first tab :nth-child(2)         
  var tabFirst = $('#'+ this.id +' a:first');
  tabFirst.tab('show');
 },
 Reload:function(){
 
  this._tabs= $("#" +this.id).bootstrapDynamicTabs();
 },
GetTabContent : function (npos) {
	var idContent = document.getElementById(this._idContent);
	if(typeof idContent.children === 'object'){
		return idContent.children[npos];
	}else if(typeof idContent.childNodes === 'object'){
		return idContent.childNodes[npos];
	}
	return null;
},
 ReloadContent: function(){
    if(this.curIdCtx!=''){
        var url = new String();
        var currForm = $('#'+ this.curIdCtx );
        if(currForm){
          var frm = currForm.children(1);
	        if(frm){
	        	if(frm[0]){
		        	url = frm[0].src;
              frm[0].src = url;
	        	}
	        }
        }
    }
 },
 openUserGuide: function(){
    if(this.curIdCtx!=''){
        var currForm = $('#'+ this.curIdCtx );
        if(currForm){
          var frm = currForm.children(1);
	        if(frm){
	        	if(frm[0]){
		        	var objid = $(frm[0]).attr('objid');
					var version = $(frm[0]).attr('version');
					var lang = $(frm[0]).attr('lang');
					var src = $(frm[0]).attr('src');
					var objTmp = src.replace('..','');
					objTmp = src.replace(System.ctx,'');
					if(lang != ''){
						objTmp = objTmp + '_' + lang;
					}					
					var url= System.ctx + '/system/libs/pdfjs-1.4.20-dist/web/viewer.html?objId=' + objTmp ;
					if(System.ctx=='/'){
						url= System.ctx + '../system/libs/pdfjs-1.4.20-dist/web/viewer.html?objId=' + objTmp ;
					}
					System.OpenModal(url, 1000, 800,('Manual: ' + objid+' ' + version + ' ' + lang  ),document);
	        	}
	        }
        }
    }
 },
 Init:function(){     
  this._tabs= $("#" +this.id).bootstrapDynamicTabs();
  $("#" +this.id).on('shown.bs.tab',".dynamic-tab >a", function (e) { 
    var obj = e.target; // var previous_tab = e.relatedTarget;
    while (obj.tagName != "LI")
    obj = obj.parentNode;
    hpdq.tab.curIdCtx=obj.getAttribute('id-content'); 

});
$('[data-tip="tooltip"]').tooltip();  
 }
}