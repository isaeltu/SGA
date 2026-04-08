window.sgaUi = {
  toggleSidebar: function () {
    const sidebar = document.getElementById('sga-sidebar');
    if (!sidebar) return;
    sidebar.classList.toggle('open');
  },
  downloadTextFile: function (filename, content) {
    const blob = new Blob([content], { type: 'text/plain;charset=utf-8' });
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = filename;
    document.body.appendChild(a);
    a.click();
    a.remove();
    URL.revokeObjectURL(url);
  },
  downloadBase64File: function (filename, contentType, base64Data) {
    const link = document.createElement('a');
    link.href = `data:${contentType};base64,${base64Data}`;
    link.download = filename;
    document.body.appendChild(link);
    link.click();
    link.remove();
  }
};
