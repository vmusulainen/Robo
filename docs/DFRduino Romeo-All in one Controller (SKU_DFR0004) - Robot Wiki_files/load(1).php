function isCompatible(ua){if(ua===undefined){ua=navigator.userAgent;}return!((ua.indexOf('MSIE')!==-1&&parseFloat(ua.split('MSIE')[1])<6)||(ua.indexOf('Firefox/')!==-1&&parseFloat(ua.split('Firefox/')[1])<3)||ua.match(/BlackBerry[^\/]*\/[1-5]\./)||ua.match(/webOS\/1\.[0-4]/)||ua.match(/PlayStation/i)||ua.match(/SymbianOS|Series60/)||ua.match(/NetFront/)||ua.match(/Opera Mini/)||ua.match(/S40OviBrowser/));}var startUp=function(){mw.config=new mw.Map(true);mw.loader.addSource({"local":{"loadScript":"/wiki/load.php","apiScript":"/wiki/api.php"}});mw.loader.register([["site","1457082441",[],"site"],["noscript","1457082441",[],"noscript"],["startup","1457490932",[],"startup"],["filepage","1457082441"],["user.groups","1457082441",[],"user"],["user","1457082441",[],"user"],["user.cssprefs","1457082441",["mediawiki.user"],"private"],["user.options","1457082441",[],"private"],["user.tokens","1457082441",[],"private"],["mediawiki.language.data","1457082441",["mediawiki.language.init"]],[
"skins.cologneblue","1457082441"],["skins.modern","1457082441"],["skins.monobook","1457082441"],["skins.vector","1457082441"],["skins.vector.beta","1457082441"],["skins.vector.js","1457082441",["jquery.delayedBind"]],["skins.vector.collapsibleNav","1457082448",["jquery.client","jquery.cookie","jquery.tabIndex"]],["jquery","1457082441"],["jquery.appear","1457082441"],["jquery.arrowSteps","1457082441"],["jquery.async","1457082441"],["jquery.autoEllipsis","1457082441",["jquery.highlightText"]],["jquery.badge","1457082441",["mediawiki.language"]],["jquery.byteLength","1457082441"],["jquery.byteLimit","1457082441",["jquery.byteLength"]],["jquery.checkboxShiftClick","1457082441"],["jquery.chosen","1457082441"],["jquery.client","1457082441"],["jquery.color","1457082441",["jquery.colorUtil"]],["jquery.colorUtil","1457082441"],["jquery.cookie","1457082441"],["jquery.delayedBind","1457082441"],["jquery.expandableField","1457082441",["jquery.delayedBind"]],["jquery.farbtastic","1457082441",[
"jquery.colorUtil"]],["jquery.footHovzer","1457082441"],["jquery.form","1457082441"],["jquery.getAttrs","1457082441"],["jquery.hidpi","1457082441"],["jquery.highlightText","1457082441",["jquery.mwExtension"]],["jquery.hoverIntent","1457082441"],["jquery.json","1457082441"],["jquery.localize","1457082441"],["jquery.makeCollapsible","1457082448"],["jquery.mockjax","1457082441"],["jquery.mw-jump","1457082441"],["jquery.mwExtension","1457082441"],["jquery.placeholder","1457082441"],["jquery.qunit","1457082441"],["jquery.qunit.completenessTest","1457082441",["jquery.qunit"]],["jquery.spinner","1457082441"],["jquery.jStorage","1457082441",["jquery.json"]],["jquery.suggestions","1457082441",["jquery.autoEllipsis"]],["jquery.tabIndex","1457082441"],["jquery.tablesorter","1457082441",["jquery.mwExtension","mediawiki.language.months"]],["jquery.textSelection","1457082441",["jquery.client"]],["jquery.validate","1457082441"],["jquery.xmldom","1457082441"],["jquery.tipsy","1457082441"],[
"jquery.ui.core","1457082441",["jquery"],"jquery.ui"],["jquery.ui.widget","1457082441",[],"jquery.ui"],["jquery.ui.mouse","1457082441",["jquery.ui.widget"],"jquery.ui"],["jquery.ui.position","1457082441",[],"jquery.ui"],["jquery.ui.draggable","1457082441",["jquery.ui.core","jquery.ui.mouse","jquery.ui.widget"],"jquery.ui"],["jquery.ui.droppable","1457082441",["jquery.ui.core","jquery.ui.mouse","jquery.ui.widget","jquery.ui.draggable"],"jquery.ui"],["jquery.ui.resizable","1457082441",["jquery.ui.core","jquery.ui.widget","jquery.ui.mouse"],"jquery.ui"],["jquery.ui.selectable","1457082441",["jquery.ui.core","jquery.ui.widget","jquery.ui.mouse"],"jquery.ui"],["jquery.ui.sortable","1457082441",["jquery.ui.core","jquery.ui.widget","jquery.ui.mouse"],"jquery.ui"],["jquery.ui.accordion","1457082441",["jquery.ui.core","jquery.ui.widget"],"jquery.ui"],["jquery.ui.autocomplete","1457082441",["jquery.ui.core","jquery.ui.widget","jquery.ui.position"],"jquery.ui"],["jquery.ui.button","1457082441",[
"jquery.ui.core","jquery.ui.widget"],"jquery.ui"],["jquery.ui.datepicker","1457082441",["jquery.ui.core"],"jquery.ui"],["jquery.ui.dialog","1457082441",["jquery.ui.core","jquery.ui.widget","jquery.ui.button","jquery.ui.draggable","jquery.ui.mouse","jquery.ui.position","jquery.ui.resizable"],"jquery.ui"],["jquery.ui.progressbar","1457082441",["jquery.ui.core","jquery.ui.widget"],"jquery.ui"],["jquery.ui.slider","1457082441",["jquery.ui.core","jquery.ui.widget","jquery.ui.mouse"],"jquery.ui"],["jquery.ui.tabs","1457082441",["jquery.ui.core","jquery.ui.widget"],"jquery.ui"],["jquery.effects.core","1457082441",["jquery"],"jquery.ui"],["jquery.effects.blind","1457082441",["jquery.effects.core"],"jquery.ui"],["jquery.effects.bounce","1457082441",["jquery.effects.core"],"jquery.ui"],["jquery.effects.clip","1457082441",["jquery.effects.core"],"jquery.ui"],["jquery.effects.drop","1457082441",["jquery.effects.core"],"jquery.ui"],["jquery.effects.explode","1457082441",["jquery.effects.core"],
"jquery.ui"],["jquery.effects.fade","1457082441",["jquery.effects.core"],"jquery.ui"],["jquery.effects.fold","1457082441",["jquery.effects.core"],"jquery.ui"],["jquery.effects.highlight","1457082441",["jquery.effects.core"],"jquery.ui"],["jquery.effects.pulsate","1457082441",["jquery.effects.core"],"jquery.ui"],["jquery.effects.scale","1457082441",["jquery.effects.core"],"jquery.ui"],["jquery.effects.shake","1457082441",["jquery.effects.core"],"jquery.ui"],["jquery.effects.slide","1457082441",["jquery.effects.core"],"jquery.ui"],["jquery.effects.transfer","1457082441",["jquery.effects.core"],"jquery.ui"],["mediawiki","1457082441"],["mediawiki.api","1457082441",["mediawiki.util"]],["mediawiki.api.category","1457082441",["mediawiki.api","mediawiki.Title"]],["mediawiki.api.edit","1457082441",["mediawiki.api","mediawiki.Title"]],["mediawiki.api.login","1457082441",["mediawiki.api"]],["mediawiki.api.parse","1457082441",["mediawiki.api"]],["mediawiki.api.watch","1457082441",["mediawiki.api",
"user.tokens"]],["mediawiki.icon","1457082441"],["mediawiki.debug","1457082441",["jquery.footHovzer"]],["mediawiki.debug.init","1457082441",["mediawiki.debug"]],["mediawiki.inspect","1457082441",["jquery.byteLength","jquery.json"]],["mediawiki.feedback","1457082441",["mediawiki.api.edit","mediawiki.Title","mediawiki.jqueryMsg","jquery.ui.dialog"]],["mediawiki.hidpi","1457082441",["jquery.hidpi"]],["mediawiki.htmlform","1457249082"],["mediawiki.notification","1457082441",["mediawiki.page.startup"]],["mediawiki.notify","1457082441"],["mediawiki.searchSuggest","1457082448",["jquery.autoEllipsis","jquery.client","jquery.placeholder","jquery.suggestions","mediawiki.api"]],["mediawiki.Title","1457082441",["jquery.byteLength","mediawiki.util"]],["mediawiki.Uri","1457082441"],["mediawiki.user","1457082441",["jquery.cookie","mediawiki.api","user.options","user.tokens"]],["mediawiki.util","1457082448",["jquery.client","jquery.cookie","jquery.mwExtension","mediawiki.notify"]],[
"mediawiki.action.edit","1457082441",["mediawiki.action.edit.styles","jquery.textSelection","jquery.byteLimit"]],["mediawiki.action.edit.styles","1457082441"],["mediawiki.action.edit.collapsibleFooter","1457082441",["jquery.makeCollapsible","mediawiki.icon"]],["mediawiki.action.edit.preview","1457082441",["jquery.form","jquery.spinner","mediawiki.action.history.diff"]],["mediawiki.action.history","1457082441",[],"mediawiki.action.history"],["mediawiki.action.history.diff","1457082441",[],"mediawiki.action.history"],["mediawiki.action.view.dblClickEdit","1457082441",["mediawiki.util","mediawiki.page.startup"]],["mediawiki.action.view.metadata","1457082955"],["mediawiki.action.view.postEdit","1457082448",["jquery.cookie","mediawiki.jqueryMsg"]],["mediawiki.action.view.rightClickEdit","1457082441"],["mediawiki.action.edit.editWarning","1457082858"],["mediawiki.action.watch.ajax","1457082441",["mediawiki.page.watch.ajax"]],["mediawiki.language","1457082441",["mediawiki.language.data",
"mediawiki.cldr"]],["mediawiki.cldr","1457082441",["mediawiki.libs.pluralruleparser"]],["mediawiki.libs.pluralruleparser","1457082441"],["mediawiki.language.init","1457082441"],["mediawiki.jqueryMsg","1457082441",["mediawiki.util","mediawiki.language"]],["mediawiki.language.months","1457082441",["mediawiki.language"]],["mediawiki.libs.jpegmeta","1457082441"],["mediawiki.page.gallery","1457082441"],["mediawiki.page.ready","1457082441",["jquery.checkboxShiftClick","jquery.makeCollapsible","jquery.placeholder","jquery.mw-jump","mediawiki.util"]],["mediawiki.page.startup","1457082441",["jquery.client","mediawiki.util"]],["mediawiki.page.patrol.ajax","1457082441",["mediawiki.page.startup","mediawiki.api","mediawiki.util","mediawiki.Title","mediawiki.notify","jquery.spinner","user.tokens"]],["mediawiki.page.watch.ajax","1457082441",["mediawiki.page.startup","mediawiki.api.watch","mediawiki.util","mediawiki.notify","jquery.mwExtension"]],["mediawiki.page.image.pagination","1457082441",[
"jquery.spinner"]],["mediawiki.special","1457082441"],["mediawiki.special.block","1457082441",["mediawiki.util"]],["mediawiki.special.changeemail","1457082441",["mediawiki.util"]],["mediawiki.special.changeslist","1457082441"],["mediawiki.special.changeslist.enhanced","1457082441"],["mediawiki.special.movePage","1457082441",["jquery.byteLimit"]],["mediawiki.special.pagesWithProp","1457082441"],["mediawiki.special.preferences","1457082441"],["mediawiki.special.recentchanges","1457082441",["mediawiki.special"]],["mediawiki.special.search","1457086605"],["mediawiki.special.undelete","1457082441"],["mediawiki.special.upload","1457490932",["mediawiki.libs.jpegmeta","mediawiki.util"]],["mediawiki.special.userlogin","1457082441"],["mediawiki.special.createaccount","1457082441"],["mediawiki.special.createaccount.js","1457082441",["mediawiki.jqueryMsg"]],["mediawiki.special.javaScriptTest","1457082441",["jquery.qunit"]],["mediawiki.tests.qunit.testrunner","1457082441",["jquery.getAttrs",
"jquery.qunit","jquery.qunit.completenessTest","mediawiki.page.startup","mediawiki.page.ready"]],["mediawiki.legacy.ajax","1457082441",["mediawiki.util","mediawiki.legacy.wikibits"]],["mediawiki.legacy.commonPrint","1457082441"],["mediawiki.legacy.config","1457082441",["mediawiki.legacy.wikibits"]],["mediawiki.legacy.protect","1457082441",["jquery.byteLimit"]],["mediawiki.legacy.shared","1457082441"],["mediawiki.legacy.oldshared","1457082441"],["mediawiki.legacy.upload","1457082441",["jquery.spinner","mediawiki.api","mediawiki.Title","mediawiki.util"]],["mediawiki.legacy.wikibits","1457082441",["mediawiki.util"]],["mediawiki.ui","1457082441"]]);mw.config.set({"wgLoadScript":"/wiki/load.php","debug":false,"skin":"vector","stylepath":"/wiki/skins","wgUrlProtocols":
"http\\:\\/\\/|https\\:\\/\\/|ftp\\:\\/\\/|ftps\\:\\/\\/|ssh\\:\\/\\/|sftp\\:\\/\\/|irc\\:\\/\\/|ircs\\:\\/\\/|xmpp\\:|sip\\:|sips\\:|gopher\\:\\/\\/|telnet\\:\\/\\/|nntp\\:\\/\\/|worldwind\\:\\/\\/|mailto\\:|tel\\:|sms\\:|news\\:|svn\\:\\/\\/|git\\:\\/\\/|mms\\:\\/\\/|bitcoin\\:|magnet\\:|urn\\:|geo\\:|\\/\\/","wgArticlePath":"/wiki/index.php?title=$1","wgScriptPath":"/wiki","wgScriptExtension":".php","wgScript":"/wiki/index.php","wgVariantArticlePath":false,"wgActionPaths":{},"wgServer":"http://www.dfrobot.com","wgUserLanguage":"en","wgContentLanguage":"en","wgVersion":"1.22.1","wgEnableAPI":true,"wgEnableWriteAPI":true,"wgMainPageTitle":"Main Page","wgFormattedNamespaces":{"-2":"Media","-1":"Special","0":"","1":"Talk","2":"User","3":"User talk","4":"Robot Wiki","5":"Robot Wiki talk","6":"File","7":"File talk","8":"MediaWiki","9":"MediaWiki talk","10":"Template","11":"Template talk","12":"Help","13":"Help talk","14":"Category","15":"Category talk"},"wgNamespaceIds":{"media":-2,
"special":-1,"":0,"talk":1,"user":2,"user_talk":3,"robot wiki":4,"robot_wiki_talk":5,"file":6,"file_talk":7,"mediawiki":8,"mediawiki_talk":9,"template":10,"template_talk":11,"help":12,"help_talk":13,"category":14,"category_talk":15,"image":6,"image_talk":7,"project":4,"project_talk":5},"wgSiteName":"Robot Wiki","wgFileExtensions":["png","gif","jpg","jpeg"],"wgDBname":"df_wikiv2","wgFileCanRotate":true,"wgAvailableSkins":{"cologneblue":"CologneBlue","vector":"Vector","modern":"Modern","monobook":"MonoBook"},"wgExtensionAssetsPath":"/wiki/extensions","wgCookiePrefix":"df_wikiv2_wk","wgResourceLoaderMaxQueryLength":-1,"wgCaseSensitiveNamespaces":[],"wgLegalTitleChars":" %!\"$\u0026'()*,\\-./0-9:;=?@A-Z\\\\\\^_`a-z~+\\u0080-\\uFFFF"});};if(isCompatible()){document.write("\u003Cscript src=\"/wiki/load.php?debug=false\u0026amp;lang=en\u0026amp;modules=jquery%2Cmediawiki\u0026amp;only=scripts\u0026amp;skin=vector\u0026amp;version=20140114T055044Z\"\u003E\u003C/script\u003E");}delete
isCompatible;
/* cache key: df_wikiv2-wk:resourceloader:filter:minify-js:7:c4ebb90b96800b3b69607029d4c1ab71 */