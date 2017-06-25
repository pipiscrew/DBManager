/**
* @link https://pipiscrew.com
* @copyright Copyright (c) 2017 PipisCrew
*/

(function( $ ) {
  
    $.fn.getSelected = function() {
            var arr = new Array();
             
            this.children('a').filter(function(){
                return $(this).attr('data-name')!=null && $(this).hasClass('list-group-item active');
            }).each(function(){
                arr.push($(this).attr('data-name'));
            });
 
            return arr;
    };
  
    $.fn.setSelected = function(jsonArray, idColName){
            for (var i = 0; i < jsonArray.length; i++)
                this.children('a').each( function(index, element){
                    if ($(this).attr('data-name')==jsonArray[i][idColName])
                    {   
                        $(this).addClass('list-group-item active');
                        return false; //exit for each
                    }
                });
    }
	
    $.fn.setAll = function(val){
            this.children('a').filter(function(){
                return $(this).attr('data-name')!=null && $(this).hasClass('list-group-item');
            }).each(function(){
				console.log(val);
				if (!val)
					$(this).removeClass('list-group-item active').addClass('list-group-item');
				else
					$(this).addClass('list-group-item active');
            });
    }
     
    $.fn.fillList = function(jsonArray, header, idColName, DescrColName, greenHeader) {
         
                if (!greenHeader)
                    var cats = "<a class='list-group-item active'> " + header + " : </a>";
                else 
                    var cats = "<a class='list-group-item active' style='background-color: #00a65a;'> " + header + " : </a>";
 
                for (var i = 0; i < jsonArray.length; i++)
                    cats += "<a href='#' class='list-group-item' data-name='" + jsonArray[i][idColName] + "'>" + jsonArray[i][DescrColName] + "</a>";
                 
                //set result-rows to element
                this.html(cats);    
    };
     
    $.fn.clearList = function() {
            this.children('a').filter(function(){
               return $(this).attr('data-name')==null;
            }).siblings().removeClass('list-group-item active').addClass('list-group-item');
    };
  
}( jQuery ));
 
$.fn.chooser = function() {
        $(this).on('click', 'a', function(event) {
                        event.preventDefault();
                         
                        if (!$(this).attr('data-name'))
                            return;
 
                        if ($(this).hasClass('list-group-item active')) {
                            $(this).removeClass('list-group-item active');
                            $(this).addClass('list-group-item');
                        } else {
                            $(this).addClass('list-group-item active');
                        }
        });
         
}