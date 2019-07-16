"use strict";

/************************************************
					Customize system theme js
************************************************/

const settings = JSON.parse(localStorage.getItem("settings"));

const tManager = new TemplateManager();

//Override hide column
$(".table")
	.on("preInit.dt", function () {
		const content = tManager.render("template_headListActions", "");
		const selector = $("div.CustomTableHeadBar");
		selector.html(content);
		window.forceTranslate("div.CustomTableHeadBar");
	});

TableColumnsVisibility.prototype.modalContainer = "#hiddenColumnsModal * .modal-body";


TableColumnsVisibility.prototype.renderCheckBox = function (data, id, vis) {
	const title = (data.targets === "no-sort") ? "#" : data.sTitle;
	return `<div class="custom-control custom-checkbox">
            	<input type="checkbox" ${vis} data-table="${id}" id="_check_${data.idx}" class="custom-control-input vis-check" data-id="${data.idx}" required />
              <label class="custom-control-label" for="_check_${data.idx}">${title}</label>
          </div>`;
};

TableColumnsVisibility.prototype.init = function (ctx) {
	const cols = this.getVisibility(`#${$(ctx).attr("id")}`);
	$(`#${$(ctx).attr("id")}`).DataTable().columns(cols.visibledItems).visible(true);
	$(`#${$(ctx).attr("id")}`).DataTable().columns(cols.hiddenItems).visible(false);
	$(".hidden-columns-event").attr("data-id", `#${$(ctx)[0].id}`);
	this.registerInitEvents();
};

TableColumnsVisibility.prototype.registerInitEvents = function () {
	$('.table-search').keyup(function () {
		const oTable = $(this).closest(".card").find(".dynamic-table").DataTable();
		oTable.search($(this).val()).draw();
	});

	//Delete multiple rows
	$(".deleteMultipleRows").on("click", function () {
		const cTable = $(this).closest(".card").find(".dynamic-table");
		if (cTable) {
			if (typeof TableBuilder !== 'undefined') {
				new TableBuilder().deleteSelectedRowsHandler(cTable.DataTable());
			}
		}
	});

	$(".add_new_inline").on("click", function () {
		new TableInlineEdit().addNewHandler(this);
	});

	//Items on page
	$(".tablePaginationView a").on("click", function () {
		const ctx = $(this);
		const onPageValue = ctx.data("page");
		const onPageText = ctx.text();
		ctx.closest(".dropdown").find(".page-size").html(`(${onPageText})`);
		const table = ctx.closest(".card").find(".dynamic-table").DataTable();
		table.page.len(onPageValue).draw();
	});

	//hide columns
	$(".hidden-columns-event").click(function () {
		new TableColumnsVisibility().toggleRightListSideBar($(this).attr("data-id"));
		$("#hiddenColumnsModal").modal();
	});
};
if (typeof TableBuilder !== 'undefined') {
	//Override table select
	TableBuilder.prototype.dom = '<"CustomTableHeadBar">rtip';
	RenderTableSelect.prototype.settings.classNameText = 'no-sort';
	RenderTableSelect.prototype.settings.select.selector = "td:not(.not-selectable):first-child .checkbox-container";
	RenderTableSelect.prototype.selectHandler = function (context) {
		const row = $(context).closest('tr');
		const table = row.closest("table").DataTable();
		if (row.hasClass("selected")) {
			table.row(row).deselect();
		} else {
			table.row(row).select();
		}
	};

	RenderTableSelect.prototype.selectHeadHandler = function (context) {
		const table = $(context).closest(".card").find(".dynamic-table");
		const dTable = table.DataTable();
		const rows = table.find("tbody tr");
		const isChecked = $(context).prop("checked");
		rows.each((index, item) => {
			const input = $(item).find("td div.checkbox-container").find("input");
			if (isChecked) {
				dTable.row(item).select();
			} else {
				dTable.row(item).deselect();
			}
			if (input) input.prop("checked", isChecked);
		});
	};

	RenderTableSelect.prototype.selectTemplateCommom = function (id, handler) {
		return `<div class="checkbox-container">
                                <div class="custom-control custom-checkbox">
                                    <input type="checkbox" onchange="${handler}" class="custom-control-input" id="_select_${id}"
                                           required>
                                    <label class="custom-control-label" for="_select_${id}"></label>
                                </div>
                            </div>`;
	};

	RenderTableSelect.prototype.settings.headContent = () => {
		const id = st.newGuid();
		return new RenderTableSelect().selectTemplateCommom(id, "new RenderTableSelect().selectHeadHandler(this)");
	};

	RenderTableSelect.prototype.templateSelect = function (data, type, row, meta) {
		const id = st.newGuid();
		return new RenderTableSelect().selectTemplateCommom(id, "new RenderTableSelect().selectHandler(this)");
	};

	//Table actions
	TableBuilder.prototype.getTableRowDeleteRestoreActionButton = function (row, dataX) {
		return `${dataX.hasDeleteRestore
			? `${row.isDeleted
				? `<a title="${window.translate("restore")}" href="javascript:void(0)" onclick="new TableBuilder().restoreItem('${row.id
				}', '#${dataX.listId}', '${dataX.viewmodelData.result.id}')"><i class="material-icons">restore</i></a>`
				: `<a title="${window.translate("delete")}" href="javascript:void(0)" onclick="new TableBuilder().deleteItem('${row.id
				}', '#${dataX.listId}', '${dataX.viewmodelData.result.id}')"><i class="material-icons">delete</i></a>`}`
			: ``}`;
	};


	TableBuilder.prototype.getTableRowInlineActionButton = function (row, dataX) {
		if (row.isDeleted) return "";
		return `${dataX.hasInlineEdit
			? `	<a title="${window.translate("edit")}" class="inline-edit" data-viewmodel="${dataX.viewmodelData.result.id
			}" href="javascript:void(0)"><i class="material-icons">edit</i></a>`
			: ``}`;
	};

	TableBuilder.prototype.getTableDeleteForeverActionButton = function (row, dataX) {
		return `${dataX.hasDeleteForever
			? `	<a onclick="new TableBuilder().deleteItemForever('${row.id
			}', '#${dataX.listId}', '${dataX.viewmodelData.result.id}')" data-viewmodel="${dataX.viewmodelData.result.id
			}" class="delete-forever" href="javascript:void(0)" title="Delete forever"><i class="material-icons">delete_forever</i></a>`
			: ``}`;
	};


	TableBuilder.prototype.getTableRowEditActionButton = function (row, dataX) {
		if (row.isDeleted) return "";
		return `${dataX.hasEditPage ? `<a href="${dataX.editPageLink}?itemId=${row.id
			}&&listId=${dataX.viewmodelData.result.id}"><i class="material-icons">edit</i></a>`
			: ``}`;
	};

	TableBuilder.prototype.replaceTableSystemTranslations = function () {
		const customReplace = new Array();
		customReplace.push({ Key: "sProcessing", Value: `<div class="col-md"><div class="lds-dual-ring"></div></div>` });
		customReplace.push({ Key: "processing", Value: `<div class="col-md"><div class="lds-dual-ring"></div></div>` });
		const serialData = JSON.stringify(customReplace);
		return serialData;
	};
}



//override inline edit templates
if (typeof TableInlineEdit !== 'undefined') {
	TableInlineEdit.prototype.toggleVisibilityColumnsButton = function (ctx, state) {
		return;
	};

	TableInlineEdit.prototype.renderActiveInlineButton = function (ctx) {
		ctx.find("i").html("check");
	};

	//Set actions for table
	TableInlineEdit.prototype.getActionsOnAdd = function () {
		const template = `<div class="btn-group" role="group" aria-label="Action buttons">
							<a href="javascript:void(0)" class='add-new-item'><i class="material-icons">check</i></a>
							<a href="javascript:void(0)" class='cancel-new-item'><i class="material-icons">cancel</i></a>
						</div>`;
		return template;
	};

	//On add new cell for inline edit
	TableInlineEdit.prototype.onGetNewAddCell = function (cell) {
		const ctx = $(cell);
		ctx.addClass("expandable-cell");
		ctx.find("div:first-child").addClass("hasTooltip");
	};

	/**
	 * On table init complete
	 * @param {any} settings
	 * @param {any} json
	 */
	TableBuilder.prototype.onInitComplete = function (settings, json) {
		//console.log(settings, json);
	}

	/**
 * On row create
 * @param {any} row
 * @param {any} data
 * @param {any} dataIndex
 */
	TableBuilder.prototype.onRowCreate = function (row, data, dataIndex) {
		const scope = this;
		if (data.isDeleted) {
			$(row).addClass("row-deleted");
			$(row).find("td.select-checkbox").find("input").css("display", "none");
			$(row).find("td").addClass("not-selectable");
		}
		const rowScope = $(row);
		
		rowScope.on("dblclick", function () {
			rowScope.attr("data-viewmodel", scope.configurations.viewmodelId);
			new TableInlineEdit().initInlineEditForRow(this);
		});
	};

	//Restyle inline edit controls
	TableInlineEdit.prototype.getBooleanEditCell = (data) => {
		const labelIdentifier = `label_${new ST().newGuid()}`;
		const container = document.createElement("div");
		container.setAttribute("class", "checkbox-container pt-2 pb-2");
		const div = document.createElement("div");
		div.setAttribute("class", "custom-control custom-checkbox");
		const label = document.createElement("label");
		label.setAttribute("class", "custom-control-label");
		label.setAttribute("for", labelIdentifier);

		const el = document.createElement("input");
		el.setAttribute("class", "inline-update-event data-input custom-control-input");
		el.setAttribute("data-prop-id", data.propId);
		el.setAttribute("data-id", data.cellId);
		el.setAttribute("type", "checkbox");
		el.setAttribute("data-prop-name", data.propName);
		el.setAttribute("data-entity", data.tableId);
		el.setAttribute("data-type", "bool");
		el.setAttribute("id", labelIdentifier);
		el.setAttribute("name", labelIdentifier);

		if (data.value) {
			el.setAttribute("checked", "checked");
		}

		div.appendChild(el);
		div.appendChild(label);
		container.appendChild(div);
		return container;
	};



	/**
 * Transform row in inline edit mode
 * @param {any} target
 */
	TableInlineEdit.prototype.initInlineEditForRow = function (target) {
		const targetCtx = $(target);
		this.renderActiveInlineButton(targetCtx);
		targetCtx.removeClass("inline-edit");
		targetCtx.addClass("inline-complete");

		const viewModelId = targetCtx.attr("data-viewmodel");
		loadAsync(`/InlineEdit/GetViewModelColumnTypes?viewModelId=${viewModelId}`).then(viewModel => {
			const dt = targetCtx.closest("table");		
			const table = dt.DataTable();
			const row = targetCtx;
			const columns = row.find(".data-cell");
			const index = table.row(row).index();
			let obj = table.row(index).data();
			for (let i = 0; i < columns.length; i++) {
				const columnCtx = $(columns[i]);
				const columnId = columnCtx.attr("data-column-id");
				const fieldData = viewModel.result.entityFields.find(x => {
					return x.columnId === columnId;
				});

				const viewModelConfigurations = viewModel.result.viewModelFields.find(x => {
					return x.id === columnId;
				});

				const cellId = columnCtx.attr("data-id");

				if (fieldData) {
					const tableId = fieldData.tableId;
					const propId = fieldData.id;
					const propName = fieldData.name;
					const parsedPropName = propName.toLowerFirstLetter();
					const value = obj[parsedPropName];
					const allowNull = fieldData.allowNull;
					let container = value;
					const data = { cellId, tableId, propId, value, propName, allowNull };
					switch (fieldData.dataType) {
						case "nvarchar":
							{
								container = this.getTextEditCell(data);
								columnCtx.html(container);
								this.onAfterInitTextEditCell(columns, i);
							}
							break;
						case "int32":
						case "decimal":
							{
								container = this.getNumberEditCell(data);
								columnCtx.html(container);
								this.onAfterInitNumberEditCell(columns, i);
							}
							break;
						case "bool":
							{
								container = this.getBooleanEditCell(data);
								columnCtx.html(container);
								this.onAfterInitBooleanEditCell(columns, i);
							}
							break;
						case "datetime":
						case "date":
							{
								container = this.getDateEditCell(data);
								columnCtx.html(container);
								this.onAfterInitDateEditCell(columns, i);
							}
							break;
						case "uniqueidentifier":
							{
								container = this.getReferenceEditCell(data);
								columnCtx.html(container);
								this.onAfterInitReferenceCell(columns, i);
							}
							break;
					}
				} else if (viewModelConfigurations.configurations.length > 0) {
					switch (viewModelConfigurations.virtualDataType) {
						//Many to many
						case 3:
							{
								this.initManyToManyControl({
									viewModelConfigurations, columnCtx, cellId
								});
							}
							break;
					}
				} else {
					this.getOnNonRecognizedField(columnCtx, viewModelConfigurations);
				}
			}
			this.bindEventsAfterInitInlineEdit(row);
		}).catch(err => {
			console.warn(err);
		});
	};


	/**
 * Transform row from edit mode to read mode
 * @param {any} target
 */
	TableInlineEdit.prototype.completeInlineEditForRow = function (target) {
		const targetCtx = $(target);
		const table = targetCtx.closest("table").DataTable();
		const row = targetCtx.closest("tr");
		const index = table.row(row).index();
		let obj = table.row(index).data();
		targetCtx.off("click", completeEditInlineHandler);
		const columns = targetCtx.parent().parent().parent().find(".data-cell");

		const viewModelId = $(columns[0]).attr("data-viewmodel");
		loadAsync(`/InlineEdit/GetViewModelColumnTypes?viewModelId=${viewModelId}`).then(viewModel => {
			if (!viewModelId) return;
			if (!viewModel.is_success) return;
			const promises = [];
			for (let i = 0; i < columns.length; i++) {
				const forPromise = new Promise((globalResolve, globalReject) => {
					const columnCtx = $(columns[i]);
					const inspect = columnCtx.find(".data-input");
					const type = inspect.attr("data-type");
					const columnId = inspect.attr("data-prop-id");
					const colId = columnCtx.attr("data-column-id");
					const threads = [];
					const fieldData = viewModel.result.entityFields.find(obj => {
						return obj.id === columnId;
					});

					const viewModelConfigurations = viewModel.result.viewModelFields.find(x => {
						return x.id === colId;
					});

					if (!inspect) globalResolve();
					const pr1 = new Promise((pr1Resolve, pr2Reject) => {
						if (!fieldData) pr1Resolve();
						const propName = fieldData.name;
						const parsedPropName = propName.toLowerFirstLetter();

						const value = inspect.val();

						switch (type) {
							case "bool":
								{
									obj[parsedPropName] = inspect.prop("checked");
									pr1Resolve();
								}
								break;
							case "uniqueidentifier":
								{
									const refEntity = inspect.attr("data-ref-entity");
									this.db.getByIdWithIncludesAsync(refEntity, value).then(refObject => {
										if (refObject.is_success) {
											obj[`${parsedPropName}Reference`] = refObject.result;
											obj[parsedPropName] = value;
										} else {
											this.toast.notifyErrorList(refObject.error_keys);
										}
										pr1Resolve();
									}).catch(err => { console.warn(err) });
								}
								break;
							default:
								{
									obj[parsedPropName] = value;
									pr1Resolve();
								}
								break;
						}
					}).then(() => {
						columnCtx.find(".inline-update-event").off("blur", onInputEventHandler);
						columnCtx.find(".inline-update-event").off("changed", onInputEventHandler);
					});
					threads.push(pr1);
					const pr2 = new Promise((localResolve, localReject) => {
						if (viewModelConfigurations) {
							switch (viewModelConfigurations.virtualDataType) {
								//Many to many
								case 3:
									{
										const { sourceEntity, sourceSelfParamName, sourceRefParamName, referenceEntityName } =
											this.getManyToManyViewModelConfigurations(viewModelConfigurations);
										const filters = [{ parameter: sourceSelfParamName.value, value: obj.id }];
										this.db.getAllWhereWithIncludesAsync(sourceEntity.value, filters).then(mResult => {
											if (mResult.is_success) {
												obj[`${sourceEntity.value.toLowerFirstLetter()}Reference`] = mResult.result;
											} else {
												this.toast.notifyErrorList(mResult.error_keys);
											}
											localResolve();
										}).catch(err => {
											console.warn(err);
											localResolve();
										});
									}
									break;
								default:
									localResolve();
									break;
							}
						} else localResolve();
					});
					threads.push(pr2);
					Promise.all(threads).then(() => {
						globalResolve();
					});
				});

				promises.push(forPromise);
			}
			promises.push(this.forceLoadDependenciesOnEditComplete(obj));
			Promise.all(promises).then(results => {
				row.find("td").unbind();
				const additionalDependencies = results[results.length - 1];
				obj = Object.assign(obj, additionalDependencies);
				const redraw = table.row(index).data(obj).invalidate();
				$(redraw.row(index).nodes()).on("dblclick", function () {
					new TableInlineEdit().initInlineEditForRow(this);
				});
			});

		}).catch(err => {
			console.warn(err);
		});
	};

	//bind events after inline edit was started for row
	TableInlineEdit.prototype.bindEventsAfterInitInlineEdit = function (row) {
		try {
			// ReSharper disable once ConstructorCallNotUsed
			new $.Iso.InlineEditingCells();
		} catch (e) { };

		$(row).unbind();
		row.on("dblclick", function () {
			$(this).unbind();
			new TableInlineEdit().completeInlineEditForRow(this);
		});
	};
}


//override notification populate container
Notificator.prototype.addNewNotificationToContainer = function (notification) {
	const _ = $("#notificationAlarm");
	if (!_.hasClass("notification"))
		_.addClass("notification");
	const template = this.createNotificationBodyContainer(notification);
	$("#notificationList").prepend(template);
	this.registerOpenNotificationEvent();
}

Notificator.prototype.createNotificationBodyContainer = function (n) {
	const block = `
		<a data-notification-id="${n.id}" href="#" class="notification-item dropdown-item py-3 border-bottom">
            <p><small>${n.subject}</small></p>
            <p class="text-muted mb-1"><small>${n.content}</small></p>
            <p class="text-muted mb-1"><small>${n.created}</small></p>
		</a>`;
	return block;
}

function getIdentifier(idt) {
	switch (idt) {
		case "en": {
			idt = "gb";
		} break;
		case "ja": {
			idt = "jp";
		} break;
		case "zh": {
			idt = "cn";
		} break;
		case "uk": {
			idt = "ua";
		} break;
		case "el": {
			idt = "gr";
		} break;
	}
	return idt;
}

function makeMenuActive(target) {
	if (target) {
		const last = target.closest("ul").closest("li");
		last.addClass("open");
		if (target.closest("nav").length !== 0)
			makeMenuActive(last);
	}
}

$(document).ready(function () {
	window.forceTranslate();
	//Log Out
	$('.sa-logout').click(function () {
		swal({
			title: window.translate("confirm_log_out_question"),
			text: window.translate("log_out_message"),
			type: "warning",
			showCancelButton: true,
			confirmButtonColor: "#DD6B55",
			confirmButtonText: window.translate("confirm_logout"),
			cancelButtonText: window.translate("cancel")
		}).then((result) => {
			if (result.value) {
				$.ajax({
					url: '/Account/LocalLogout',
					type: "post",
					dataType: "json",
					contentType: "application/x-www-form-urlencoded; charset=utf-8",
					success: function (data) {
						if (data.success) {

							swal("Success!", data.message, "success");
							window.location.href = '/Account/Login';
						} else {
							swal("Fail!", data.message, "error");
						}
					},
					error: function () {
						swal("Fail!", "Server no response!", "error");
					}
				});
			};
		});
	});

	//Menu render promise
	const loadMenusPromise = new Promise((resolve, reject) => {
		const menus = load("/PageRender/GetMenus");
		resolve(menus);
	});

	loadMenusPromise.then(menus => {
		const renderMenuContainer = $("#left-nav-bar");
		if (menus.is_success) {
			const content = tManager.render("template_RenderIsoMenuItem.html", menus.result, {
				host: location.origin
			});
			renderMenuContainer.html(content);
			let route = location.href;
			if (route[route.length - 1] === "#") {
				route = route.substr(0, route.length - 1);
			}
			renderMenuContainer.find(`a[href='${route}']`)
				.parent()
				.addClass("active");
			makeMenuActive(renderMenuContainer.find(`a[href='${route}']`));
			window.forceTranslate("#left-nav-bar");
			const $body = $("body");
			$body.delegate('.navigation li:has(.sub-nav) > a', 'click', function () {
				/*$('.navigation li').removeClass('open');*/
				$(this).siblings('.sub-nav').slideToggle();
				$(this).parent().toggleClass('open');
				return false;
			});
			$body.find('.navigation ul li:has(.sub-nav)').on('mouseover', function () {
				if ($(".sidebar").hasClass("collapsed")) {
					const $menuItem = $(this),
						$submenuWrapper = $('> .sub-nav', $menuItem);
					// grab the menu item's position relative to its positioned parent
					const menuItemPos = $menuItem.position();

					// place the submenu in the correct position relevant to the menu item
					$submenuWrapper.css({
						top: menuItemPos.top,
						left: menuItemPos.left + $menuItem.outerWidth()
					});
				}
			});
		}
	});


	//Localization promise
	var localizationPromise = new Promise((resolve, reject) => {
		//Set localization config
		let translateIcon = getIdentifier(settings.localization.current.identifier);
		$("#currentlanguage").addClass(`flag-icon flag-icon-${translateIcon}`);
		const languageBlock = $("#languageRegion");
		resolve(languageBlock);
	});

	localizationPromise.then(languageBlock => {
		$.each(settings.localization.languages, function (index, lang) {
			const language = `<a href="/Localization/ChangeLanguage?identifier=${lang.identifier}" class="dropdown-item language-event">
							<i class="flag-icon flag-icon-${getIdentifier(lang.identifier)}"></i> ${lang.name}
						</a>`;
			languageBlock.append(language);
		});
	});

	localizationPromise.then(() => {
		$(".language-event").on("click", function () {
			localStorage.removeItem("hasLoadedTranslations");
		});
	});

	//Emails promise
	var emailPromise = new Promise((resolve, reject) => {
		const notificator = new Notificator();
		const response = notificator.getFolders();
		if (response) resolve(response);
	});

	emailPromise.then(response => {
		if (response.is_success) {
			var folders = response.result.values;
			const f = folders.find((e) => e.Name === "Inbox");
			const uri = `/Email?folderId=${f.Id}`;
			$("#SeeAllEmails").attr("href", uri);

			const content = tManager.render("template_folders_layout.html", folders);
			var m = $(".notification-items");
			m.html(content);
			$("#right_menu").html(content);
			m.find("a").on("click", function () {
				const folderId = $(this).attr("folderid");
				if (folderId != undefined) {
					window.location.href = `/Email?folderId=${folderId}`;
				}
			});
		}
	});

	Promise.all([loadMenusPromise, localizationPromise, emailPromise]).then(function (values) {
		window.forceTranslate();
	});
});








/***********************************************
 	Rewrite add addAsync for code formats
************************************************/

DataInjector.prototype.addAsync = function (entityName, object) {
	const promises = [];
	const entityCodeFormats = [
		{ name: "Objective", code: 1000, propName: "Code" },
		{ name: "Asset", code: 1001, propName: "Code" },
		{ name: "Risk", code: 1002, propName: "Code" },
		{ name: "KPI", code: 1003, propName: "Code" },
		{ name: "InternalAudit", code: 1004, propName: "Code" },
		{ name: "Meeting", code: 1005, propName: "Code" },
		{ name: "NomInterestedParty", code: 1006, propName: "Code" },
		{ name: "NomLocation", code: 1007, propName: "Code" }
	];
	const search = entityCodeFormats.find(x => x.name.toLowerCase() === entityName.toLowerCase());
	if (search) {
		const pr = new Promise((resolve, reject) => {
			this.getAllWhereWithIncludesAsync("CompanySettings").then(x => {
				if (x.is_success) {
					const config = x.result.find(y => y.parameterReference.code === search.code);
					if (config) {
						let format = config.value;
						const d = new Date();
						format = format.replace(/{Year}/g, d.getFullYear());
						format = format.replace(/{Month}/g, d.getMonth() < 10 ? `0${d.getMonth()}` : d.getMonth());
						format = format.replace(/{Day}/g, d.getDate() < 10 ? `0${d.getDate()}` : d.getDate());
						this.countAsync(entityName).then(g => {
							if (g.is_success) {
								format = format.replace(/{NextIndex}/g, g.result + 1);
								object[search.propName] = format;
								resolve();
							}
						});
					} else {
						reject(window.translate("iso_code_format_not_configured"));
					}
				}
			});
		});
		promises.push(pr);
	}

	return new Promise((resolve, reject) => {
		Promise.all(promises).then(x => {
			const dataParams = JSON.stringify({
				entityName: entityName,
				object: JSON.stringify(object)
			});
			$.ajax({
				url: `/api/DataInjector/AddAsync`,
				data: dataParams,
				method: "post",
				contentType: "application/json; charset=utf-8",
				dataType: "json",
				success: function (data) {
					resolve(data);
				},
				error: function (error) {
					reject(error);
				}
			});
		}).catch(e => {
			new ToastNotifier().notify({ heading: e });
		});
	});
};

/************************************************
					End Custom js
************************************************/

var PreLoader;

$(window).on("load", function () {
	$('.loader-wrapper').not('.incomponent').fadeOut(600, function () {
		PreLoader = $(this).detach();
	});
});


/* Dom Ready */
(function ($) {

	"use strict";

	const $body = $('body');

	/* Initialize Tooltip */
	$('[data-toggle="tooltip"]').tooltip();


	/* Initialize Popover */
	$('[data-toggle="popover"]').popover();


	/* Initialize Lightbox */
	$body.delegate('[data-toggle="lightbox"]', 'click', function (event) {
		event.preventDefault();
		$(this).ekkoLightbox();
	});


    /************************************************
     Append Preloader (use in ajax call)
     ************************************************/
	$body.delegate('.append-preloader', 'click', function () {

		$(PreLoader).show();
		$body.append(PreLoader);
		setTimeout(function () {

			$('.loader-wrapper').fadeOut(1000, function () {
				PreLoader = $(this).detach();
			});

		}, 3000);

	});


    /************************************************
     Toggle Preloader in card or box
     ************************************************/
	$body.delegate('[data-toggle="loader"]', 'click', function () {

		var target = $(this).attr('data-target');
		$('#' + target).show();

	});


    /************************************************
     Toggle Sidebar Nav
     ************************************************/
	$body.delegate('.toggle-sidebar', 'click', function () {
		$('.sidebar').toggleClass('collapsed');

		if (localStorage.getItem("asideMode") === 'collapsed') {
			localStorage.setItem("asideMode", 'expanded')
		} else {
			localStorage.setItem("asideMode", 'collapsed')
		}
		return false;
	});

	var p;
	$body.delegate('.hide-sidebar', 'click', function () {
		if (p) {
			p.prependTo(".wrapper");
			p = null;
		} else {
			p = $(".sidebar").detach();
		}
	});

	$.fn.setAsideMode = function () {
		if (localStorage.getItem("asideMode") === null) {

		} else if (localStorage.getItem("asideMode") === 'collapsed') {
			$('.sidebar').addClass('collapsed');
		} else {
			$('.sidebar').removeClass('collapsed');
		}
	};
	if ($(window).width() > 768) {
		$.fn.setAsideMode();
	}

	$body.find('.navigation ul li:has(.sub-nav)').on('mouseover', function () {
		if ($(".sidebar").hasClass("collapsed")) {
			alert();
			const $menuItem = $(this),
				$submenuWrapper = $('> .sub-nav', $menuItem);
			// grab the menu item's position relative to its positioned parent
			const menuItemPos = $menuItem.position();

			// place the submenu in the correct position relevant to the menu item
			$submenuWrapper.css({
				top: menuItemPos.top,
				left: menuItemPos.left + $menuItem.outerWidth()
			});
		}
	});

    /************************************************
     Toggle Controls on small devices
     ************************************************/
	$body.delegate('.toggle-controls', 'click', function () {
		$('.controls-wrapper').toggle().toggleClass('d-none');
	});


    /************************************************
     Toast Messages
     ************************************************/
	$body.delegate('[data-toggle="toast"]', 'click', function () {

		var dataAlignment = $(this).attr('data-alignment');
		var dataPlacement = $(this).attr('data-placement');
		var dataContent = $(this).attr('data-content');
		var dataStyle = $(this).attr('data-style');

		if ($('.toast.' + dataAlignment + '-' + dataPlacement).length) {
			$('.toast.' + dataAlignment + '-' + dataPlacement).append('<div class="alert alert-dismissible fade show alert-' + dataStyle + ' "> ' + dataContent + '<button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true" class="material-icons md-18">clear</span></button></div>');
		} else {
			$body.append('<div class="toast ' + dataAlignment + '-' + dataPlacement + '"> <div class="alert alert-dismissible fade show alert-' + dataStyle + ' "> ' + dataContent + '<button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true" class="material-icons md-18">clear</span></button></div> </div>');
		}
	});


    /**************************************
     Chosen Form Control
     **************************************/
	$('.form-control-chosen').chosen({
		allow_single_deselect: true,
		width: '100%'
	});
	$('.form-control-chosen-required').chosen({
		allow_single_deselect: false,
		width: '100%'
	});
	$('.form-control-chosen-search-threshold-100').chosen({
		allow_single_deselect: true,
		disable_search_threshold: 100,
		width: '100%'
	});
	$('.form-control-chosen-optgroup').chosen({
		width: '100%'
	});
	$(function () {
		$('[title="clickable_optgroup"]').addClass('chosen-container-optgroup-clickable');
	});
	$(document).delegate('[title="clickable_optgroup"] .group-result', 'click', function () {
		var unselected = $(this).nextUntil('.group-result').not('.result-selected');
		if (unselected.length) {
			unselected.trigger('mouseup');
		} else {
			$(this).nextUntil('.group-result').each(function () {
				$('a.search-choice-close[data-option-array-index="' + $(this).data('option-array-index') + '"]').trigger('click');
			});
		}
	});


    /*****************************************
     Themer Changer with local storage
     *****************************************/

	$.fn.removeClassStartingWith = function (filter) {
		$(this).removeClass(function (index, className) {
			return (className.match(new RegExp("\\S*" + filter + "\\S*", 'g')) || []).join(' ')
		});
		return this;
	};


	$body.delegate('.theme-changer', 'click', function () {
		var primaryColor = $(this).attr('primary-color');
		var sidebarBg = $(this).attr('sidebar-bg');
		var logoBg = $(this).attr('logo-bg');
		var headerBg = $(this).attr('header-bg');

		localStorage.setItem("primaryColor", primaryColor);
		localStorage.setItem("sidebarBg", sidebarBg);
		localStorage.setItem("logoBg", logoBg);
		localStorage.setItem("headerBg", headerBg);

		$.fn.setThemeTone(primaryColor);
	});


	$.fn.setThemeTone = function (primaryColor) {

		if (localStorage.getItem("primaryColor") === null) {

		} else {

			/* SIDEBAR */
			if (localStorage.getItem("sidebarBg") === "light") {
				$('.sidebar ').addClass('sidebar-light');
			} else {
				$('.sidebar').removeClass('sidebar-light');
			}


			/* PRIMARY COLOR */
			if (localStorage.getItem("primaryColor") === 'primary') {
				document.documentElement.style.setProperty('--theme-colors-primary', '#4B89FC');
			} else {
				var colorCode = getComputedStyle(document.body).getPropertyValue('--theme-colors-' + localStorage.getItem("primaryColor"));
				document.documentElement.style.setProperty('--theme-colors-primary', colorCode);
			}


			/* LOGO */
			if (localStorage.getItem("logoBg") === 'white' || localStorage.getItem("logoBg") === 'light') {
				$('.sidebar .navbar').removeClassStartingWith('bg').removeClassStartingWith('navbar-dark').addClass('navbar-light bg-' + localStorage.getItem("logoBg"));
			} else {
				$('.sidebar .navbar').removeClassStartingWith('bg').removeClassStartingWith('navbar-light').addClass('navbar-dark bg-' + localStorage.getItem("logoBg"));
			}


			/* HEADER */
			if (localStorage.getItem("headerBg") === "light" || localStorage.getItem("headerBg") === "white") {
				$('.header .navbar').removeClassStartingWith('bg').removeClassStartingWith('navbar-dark').addClass('navbar-light bg-' + localStorage.getItem("headerBg"));
			} else {
				$('.header .navbar').removeClassStartingWith('bg').removeClassStartingWith('navbar-light').addClass('navbar-dark bg-' + localStorage.getItem("headerBg"));
			}

		}


	};


	$.fn.setThemeTone();


})(jQuery);


/*****************************************
 Full Screen Toggle
 *****************************************/
function toggleFullScreen() {
	if ((document.fullScreenElement && document.fullScreenElement !== null) || (!document.mozFullScreen && !document.webkitIsFullScreen)) {
		if (document.documentElement.requestFullScreen) {
			document.documentElement.requestFullScreen();
		} else if (document.documentElement.mozRequestFullScreen) {
			document.documentElement.mozRequestFullScreen();
		} else if (document.documentElement.webkitRequestFullScreen) {
			document.documentElement.webkitRequestFullScreen(Element.ALLOW_KEYBOARD_INPUT);
		}
	} else {
		if (document.cancelFullScreen) {
			document.cancelFullScreen();
		} else if (document.mozCancelFullScreen) {
			document.mozCancelFullScreen();
		} else if (document.webkitCancelFullScreen) {
			document.webkitCancelFullScreen();
		}
	}
}














