window.sgaUi = {
  _activityHandler: null,
  _dotNetRef: null,
  toggleSidebar: function () {
    const sidebar = document.getElementById('sga-sidebar');
    if (!sidebar) return;
    sidebar.classList.toggle('open');
  },
  confirmAction: function (message) {
    return window.confirm(message);
  },
  sessionSet: function (key, value) {
    window.sessionStorage.setItem(key, value);
  },
  sessionGet: function (key) {
    return window.sessionStorage.getItem(key);
  },
  sessionRemove: function (key) {
    window.sessionStorage.removeItem(key);
  },
  registerActivityTracker: function (dotNetRef) {
    this.unregisterActivityTracker();

    this._dotNetRef = dotNetRef;
    this._activityHandler = () => {
      if (this._dotNetRef) {
        this._dotNetRef.invokeMethodAsync('NotifyActivity');
      }
    };

    const opts = { passive: true };
    window.addEventListener('click', this._activityHandler, opts);
    window.addEventListener('keydown', this._activityHandler, opts);
    window.addEventListener('scroll', this._activityHandler, opts);
    window.addEventListener('touchstart', this._activityHandler, opts);
  },
  unregisterActivityTracker: function () {
    if (!this._activityHandler) {
      return;
    }

    window.removeEventListener('click', this._activityHandler);
    window.removeEventListener('keydown', this._activityHandler);
    window.removeEventListener('scroll', this._activityHandler);
    window.removeEventListener('touchstart', this._activityHandler);

    this._activityHandler = null;
    this._dotNetRef = null;
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
  downloadHtmlFile: function (filename, content) {
    const blob = new Blob([content], { type: 'text/html;charset=utf-8' });
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
  },
  downloadTicketPng: async function (filename, ticket) {
    const width = 1080;
    const height = 1500;
    const canvas = document.createElement('canvas');
    canvas.width = width;
    canvas.height = height;
    const ctx = canvas.getContext('2d');
    if (!ctx) {
      return;
    }

    ctx.fillStyle = '#f4f7fb';
    ctx.fillRect(0, 0, width, height);

    const cardX = 70;
    const cardY = 60;
    const cardWidth = width - 140;
    const cardHeight = height - 120;

    ctx.fillStyle = '#ffffff';
    ctx.fillRect(cardX, cardY, cardWidth, cardHeight);
    ctx.strokeStyle = '#e5e7eb';
    ctx.lineWidth = 2;
    ctx.strokeRect(cardX, cardY, cardWidth, cardHeight);

    ctx.fillStyle = '#0b5f73';
    ctx.font = 'bold 48px Segoe UI, Arial, sans-serif';
    ctx.fillText('Reserva Confirmada', cardX + 46, cardY + 80);

    ctx.fillStyle = '#64748b';
    ctx.font = '28px Segoe UI, Arial, sans-serif';
    ctx.fillText(`Reserva #${ticket.reservationId}`, cardX + 46, cardY + 125);

    const drawLine = (label, value, y) => {
      ctx.fillStyle = '#64748b';
      ctx.font = 'bold 20px Segoe UI, Arial, sans-serif';
      ctx.fillText(label.toUpperCase(), cardX + 46, y);
      ctx.fillStyle = '#111827';
      ctx.font = '31px Segoe UI, Arial, sans-serif';
      ctx.fillText(value, cardX + 46, y + 42);
      ctx.strokeStyle = '#edf2f7';
      ctx.lineWidth = 1;
      ctx.beginPath();
      ctx.moveTo(cardX + 42, y + 66);
      ctx.lineTo(cardX + cardWidth - 42, y + 66);
      ctx.stroke();
    };

    drawLine('Ruta', ticket.route, cardY + 220);
    drawLine('Fecha del viaje', ticket.departureDate, cardY + 330);
    drawLine('Hora de salida', ticket.departureTime, cardY + 440);
    drawLine('Bus asignado', ticket.bus, cardY + 550);
    drawLine('Turno', ticket.queue, cardY + 660);
    drawLine('Codigo de reserva', ticket.reservationCode, cardY + 770);

    const qrSize = 300;
    const qrX = cardX + (cardWidth - qrSize) / 2;
    const qrY = cardY + 880;

    ctx.fillStyle = '#fafafa';
    ctx.fillRect(qrX - 20, qrY - 20, qrSize + 40, qrSize + 40);
    ctx.strokeStyle = '#d1d5db';
    ctx.lineWidth = 2;
    ctx.strokeRect(qrX - 20, qrY - 20, qrSize + 40, qrSize + 40);

    if (ticket.qrBase64) {
      const qrImage = await new Promise((resolve, reject) => {
        const image = new Image();
        image.onload = () => resolve(image);
        image.onerror = reject;
        image.src = `data:image/png;base64,${ticket.qrBase64}`;
      });

      ctx.drawImage(qrImage, qrX, qrY, qrSize, qrSize);
    }

    ctx.fillStyle = '#64748b';
    ctx.font = '28px Segoe UI, Arial, sans-serif';
    ctx.textAlign = 'center';
    ctx.fillText('Escanea este codigo al abordar', width / 2, qrY + qrSize + 62);
    ctx.textAlign = 'left';

    const link = document.createElement('a');
    link.href = canvas.toDataURL('image/png');
    link.download = filename;
    document.body.appendChild(link);
    link.click();
    link.remove();
  }
};
