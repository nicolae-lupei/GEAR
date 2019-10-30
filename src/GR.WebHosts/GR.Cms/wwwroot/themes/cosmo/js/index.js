const localizer = new Localizer();

$("#headerContainer").replaceWith($(".headContainer"));

const menuBlock = $("#navbarNavAltMarkup div:first-child");
loadAsync("/PageRender/GetMenus?menuId=b02f6702-1bfe-4fdb-8f7a-a86447620b7e").then(menus => {
	if (menus.is_success) {
		//<span class="sr-only">(current)</span>
		$.each(menus.result, (i, item) => {
			const block = `<a href="${item.href}" translate="${item.translate}" class="nav-item nav-link py-1 px-3">${item.name}</a>`;
			menuBlock.append(block);
		});
		window.forceTranslate("#navbarNavAltMarkup");
	}
	$(".preloader").fadeOut();
});

//user Section
const userSection = $("#userSection");
new Notificator().getCurrentUser().then(user => {
	if (user.is_success) {
		userSection.replaceWith($(`<div class="navbar-nav user-nav" style="margin-right: -6em; padding-right: 1em;">
        <a class="nav-item nav-link py-1 px-3" href="/home">${window.translate("iso_hello").toUpperFirstLetter()}, ${user.result.userName}</a>
        <a href="#" class="logoff btn btn-outline-primary py-2 ml-2 sa-logout">${window.translate("logout")}</a>
    </div>`));

		//Log Out
		new ST().registerLocalLogout(".sa-logout");
	}
});

$(document).ready(function () {
	const languagesBlock = $("#languages");
	window.loadAsync("/Localization/GetLanguagesAsJson").then(langs => {
		if (!langs) return;
		const currentLanguage = window.getCookie("language");
		let currentIdentifier = localizer.adaptIdentifier(langs.find(x => x.name === currentLanguage).identifier);
		if (currentIdentifier === 'gb') {
			currentIdentifier = 'en';
		}
		const b = $(`<li class="navbar-nav nav-link px-3 dropdown">
			 <a href="javascript:void(0)" class=" nav-link px-3 dropdown-toggle text-uppercase" data-toggle="dropdown">
				${currentIdentifier}
            <span class="caret"></span>
        </a>
		<ul class="dropdown-menu lang-selector">
		</ul>
		</li>`);
		$.each(langs, function (index, lang) {
			const language = `<a href="/Localization/ChangeLanguage?identifier=${lang.identifier}" class="dropdown-item language-event">
							${lang.name}
						</a>`;
			b.find(".lang-selector").append(language);
		});
		languagesBlock.append(b);
		$(".language-event").on("click", function () {
			localStorage.removeItem("hasLoadedTranslations");
		});
	});
});


//Include chat
(function (w, d) {
	w.HelpCrunch = function () { w.HelpCrunch.q.push(arguments) }; w.HelpCrunch.q = [];
	function r() { var s = document.createElement('script'); s.async = 1; s.type = 'text/javascript'; s.src = 'https://widget.helpcrunch.com/'; (d.body || d.head).appendChild(s) };
	if (w.attachEvent) { w.attachEvent('onload', r) } else { w.addEventListener('load', r, false) }
})(window, document);
HelpCrunch('init', 'iso27001expert', {
	applicationId: 1,
	applicationSecret: 'GqwtGJkruYiB8bbRmISXaHSl1DuhRdmJpXrIiAr+8omlkxJa5ef7FzgkwQuJecAvfX/MzGLSPK6pAOsEzybCzQ=='
});

HelpCrunch('showChatWidget');

$(function () {
	let index = 0;
	const helpCrunchTimer = setInterval(function () {
		try {
			const helpCrunchContainer = $($(`iframe[name='helpcrunch-iframe']`).get(0).contentDocument);
			if (helpCrunchContainer) {
				helpCrunchContainer.on("click", function () {
					setTimeout(() => {
						helpCrunchContainer.find("#helpcrunch-container.helpcrunch-chat-fadein .helpcrunch-chat").css("background", "#0540b5");
					}, 200);
				});

				helpCrunchContainer.find(".helpcrunch-widget-type-icon-label").css("background", "#0540b5");
				helpCrunchContainer.find(".helpcrunch-widget-icon-block").css("background", "#0540b5");
				helpCrunchContainer.find(".helpcrunch-widget-type-icon-triangle").css("border", "#0540b5");
				helpCrunchContainer.find(".helpcrunch-widget-type-icon-text").html(window.translate('iso_chat_what_is_your_question'));
				$(`iframe[name='helpcrunch-iframe']`).css("display", "block");
				if (index == 3) clearInterval(helpCrunchTimer);
				index++;
			}
		}
		catch (e) { }
	}, 300);
});

$(function () {
	let btn = $(".scroll-to-top");

	$(window).scroll(function () {
		if ($(window).scrollTop() > 300) {
			btn.fadeIn();
		} else {
			btn.fadeOut();
		}
		if ($(window).scrollTop() > 80) {
			$('.page-header').addClass('no-padding');
			$('.sponsors').addClass('slideup');
		} else {
			$('.page-header').removeClass('no-padding');
			$('.sponsors').removeClass('slideup');
		}
	});

	btn.on('click', function (e) {
		e.preventDefault();
		$('html, body').animate({ scrollTop: 0 }, '300');
	});

	$(document).ready(function () {
		window.forceTranslate().then(() => {
			replaceIso();
		});
		replaceIso();
	});


	function replaceIso() {
		$.each($('.iso-text'), function () {
			let str = $(this);
			if (!$(this).find('.color-blue-fw-4400').length > 0) {
				str.html(
					str.text().replace('ISO 27001', '<span><i><span class="color-blue fw-400">ISO</span> 27001</i></span>')
				);
			}
		});
	}

});