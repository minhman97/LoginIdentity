$(function () {
	var buildMeta = function () {
		datatable.dataSet = datatable.dataSet || [];
		Plugin.localDataUpdate();
		// local pagination meta
		var meta = Plugin.getDataSourceParam('pagination');
		if (meta.perpage === 0) {
			meta.perpage = options.data.pageSize || 10;
		}
		meta.total = datatable.dataSet.length;
		debugger;
		if (!options.pagingCallback) {
			var start = Math.max(meta.perpage * (meta.page - 1), 0);
			var end = Math.min(start + meta.perpage, meta.total);
			datatable.dataSet = $(datatable.dataSet).slice(start, end);
		}
		return meta;
	};
});