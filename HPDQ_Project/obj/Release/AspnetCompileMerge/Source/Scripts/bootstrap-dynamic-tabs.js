/*jslint browser: true*/
/*global jQuery*/
/*jslint vars: true*/

(function ($) {
    $.fn.bootstrapDynamicTabs = function () {

		this.each(function () {
			var $horizontalContainer = $(this);
			function DynamicTabs() {

				// create wrapper and menu if first time
				if ($horizontalContainer.parent('.dynamic-tabs-container').length === 0) {

					// add wrapper
					$horizontalContainer.addClass("dynamic-tabs").wrap("<div class='dynamic-tabs-container clearfix'></div>");

					// Attach a dropdown to the right of the tabs bar
					// This will be toggled if tabs can't fit in a given viewport size
					//$horizontalContainer.after(
					//	"<div class='nav navbar-nav navbar-right dropdown tabs-dropdown' style='padding:10px 0px'><a href='#' class='' data-toggle='dropdown'><i class='fa fa-bars' style='font-size: 1.4em;'></i></a><ul class='dropdown-menu' role='menu'><div class='dropdown-header visible-xs'><p class='count'>Tabs</p><button type='button' class='close' data-dismiss='dropdown'><span aria-hidden='true'>&times;</span></button><div class='divider visible-xs'></div></div></ul></div>"
					//	+"<div class='toolbox'><a href='#' onclick='hpdq.tab.PinOnOff(this)' data-tip='tooltip'  title='Pin'  class=''><i class='glyphicon glyphicon-pushpin'  status='1'  style='font-size: 1.4em;' ></i></a>" 
					//	+"<a href='#' onclick='hpdq.tab.ReloadContent()'  data-tip='tooltip'  title='Reload' class=''><i class='fas fa-sync-alt' style='font-size: 1.4em;'></i></a>"
					//	+"<a href='#' onclick='hpdq.tab.openUserGuide()' data-tip='tooltip'  title='Help'  class=''><i class='fa fa-question-circle' style='font-size: 1.4em;'></i></a>"
					//	+"<a href='#' onclick='hpdq.tab.RemoveTabCurr()' data-tip='tooltip'  title='Close'  class=''><i class='fa fa-window-close' style='font-size: 1.4em;'></i></a></div>"
					//);
				}

				// Mark each tab with a 'tab-id' for easy access
				var $tabs = $horizontalContainer.children('li');
				$tabs.each(function (i) {
					$(this)
						.addClass("dynamic-tab")
						.attr("tab-id", i);
				});

				// Update tabs funtion
				var updateTabs = function () {
					var i, horizontalTab, tabId, verticalTab, tabWidth, isVisible;

					var availableWidth = $horizontalContainer.width();
					var numVisibleHorizontalTabs = 1;

					var activeTab = $horizontalContainer.find('.active');
					var activeTabIndex = activeTab.index();

					// active tab is always visible
					activeTab.toggleClass('hidden', false);
					availableWidth = availableWidth - activeTab.outerWidth(true);

					// Determine which tabs to show/hide
					var $tabs = $horizontalContainer.children('li');
				

					// from active to firt
					for (i = activeTabIndex - 1; i >= 0; i--) {
						horizontalTab = $tabs.eq(i);
						if (availableWidth > 0) {
							horizontalTab.toggleClass('hidden', false);
							tabWidth = horizontalTab.outerWidth(true);
							isVisible = tabWidth <= availableWidth;
							if (isVisible) {
								availableWidth = availableWidth - tabWidth;
								numVisibleHorizontalTabs++;
							} else {
								availableWidth = -1;
							}
						} else {
							isVisible = false;
						}
						horizontalTab.toggleClass('hidden', !isVisible);
					}

					// from active to last
					for (i = activeTabIndex + 1; i < $tabs.length; i++) {
						horizontalTab = $tabs.eq(i);
						if (availableWidth > 0) {
							horizontalTab.toggleClass('hidden', false);
							tabWidth = horizontalTab.outerWidth(true);
							isVisible = tabWidth <= availableWidth;
							if (isVisible) {
								availableWidth = availableWidth - tabWidth;
								numVisibleHorizontalTabs++;
							} else {
								availableWidth = -1;
							}
						} else {
							isVisible = false;
						}
						horizontalTab.toggleClass('hidden', !isVisible);
					}

					// Toggle the Tabs dropdown if there are more tabs than can fit in the tabs horizontal container
					var numVisibleVerticalTabs = $tabs.length - numVisibleHorizontalTabs;
					var hasVerticalTabs = (numVisibleVerticalTabs > 0);
					$horizontalContainer.siblings(".tabs-dropdown").toggleClass("hidden", !hasVerticalTabs);
				};
				// tabs dropdown event
				var onDropDow = function () {
					// Clone each tab into the dropdown
					var $verticalContainer = $horizontalContainer.siblings(".tabs-dropdown").find(".dropdown-menu");

					$verticalContainer.html("");
					$verticalContainer.css("z-index", 1050);
					
					$horizontalContainer.children('li').each( function (index, element) {
						if ($(element).children('a').css("display") !== 'none') {
							var htab = document.createElement("li");
						
							$(htab).append('<a href="#">'+$(element).children('a').html()+'</a>');
							$(htab).toggleClass('active',$(element).hasClass('active'));
							$(htab).toggleClass('hidden',!$(element).hasClass('hidden'));
							$(htab).attr("tab-id", index);
							$(htab).on("click", function (e) {
								$horizontalContainer.find("[tab-id=" + $(this).attr("tab-id") + "] a").tab("show");
								updateTabs();
							});
							$verticalContainer.append(htab);
						}
					});
				};
				$horizontalContainer.siblings(".tabs-dropdown").off('show.bs.dropdown').on('show.bs.dropdown', onDropDow);

				// update tabs
				updateTabs();

				// Update tabs on window resize
				$(window).off("resize.dyntabs").on("resize.dyntabs", function () {
					updateTabs();
				});
			}

			return new DynamicTabs();
		});
		return this;
	};
}(jQuery));
