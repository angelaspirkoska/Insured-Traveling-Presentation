'use strict';

// IE shim
if (!Array.prototype.indexOf) {
    Array.prototype.indexOf = function(obj, start) {
         for (var i = (start || 0), j = this.length; i < j; i++) {
             if (this[i] === obj) { return i; }
         }
         return -1;
    }
}

// endsWith()
if (!String.prototype.endsWith) {
  String.prototype.endsWith = function(searchString, position) {
      var subjectString = this.toString();
      if (typeof position !== 'number' || !isFinite(position) || Math.floor(position) !== position || position > subjectString.length) {
        position = subjectString.length;
      }
      position -= searchString.length;
      var lastIndex = subjectString.indexOf(searchString, position);
      return lastIndex !== -1 && lastIndex === position;
  };
}

var normalMapInit = function() {
    if ( typeof(gmapInit) === 'function' ) gmapInit();
};

// Jivo online indicators
var jivo_onLoadCallback = function() {
  if (typeof (jivo_api) !== 'undefined') {
    if ( jivo_api.chatMode() === 'offline' ) {
      $('.shortcuts .chat-offline').removeClass('hidden');
    } else {
      $('.shortcuts .chat-online').removeClass('hidden');
    }
  }
};


var tSite = {

    w$ : null,

    ln: 'mk', // en, mk

    now: moment().startOf('day'),

    mapSetup: null,
    mapMarkers: [],
    markerCluster: null,

    locations: [],

    lang: {},

    DATE_FORMAT: 'D.M.YYYY',

    VALIDATOR_ICONS: {
        valid: 'fa fa-check',
        invalid: 'fa fa-times',
        validating: 'fa fa-refresh'
    },

    colors: [ '#3ca082','#8ac6b4','#c4e2d9',
              '#46494a','#909292','#c7c8c8',
              '#009cd1','#66c4e3','#b2e1f1',
              '#744b93','#ac93be','#d5c9de',
              '#dc911b','#eabd76','#f4deba',
              '#224585','#7a8fb6','#bcc7da' ],

    bornDate: {
        min: moment().startOf('day').subtract(101, 'y'),
        max: moment().startOf('day').subtract(31, 'd'),
        lgl: moment().startOf('day').subtract(18, 'y')
    },

    formatNumber: function(n, decplaces) {
        var decseparator = arguments[2] || ",";
        var thoseparator = arguments[3] || ".";

        if ( isNaN(n) ) return 0;

        var str = "" + Math.round(n * Math.pow(10,decplaces));

        while (str.length <= decplaces) {
            str = "0" + str;
        }

        var decpoint = str.length - decplaces;

        var formats = decplaces > 0 ? decseparator + str.substring(decpoint, str.length) : "";

        var i = decpoint-3;
        while (i > 0) {
            formats = thoseparator + str.substring(i, i+3) + formats
            decpoint = i;
            i -= 3;
        }
        formats = str.substring(0,decpoint) + formats;

        return formats
    },


    dateFormat: function(s, type) {
        var d = s.split("|");
        if ( d.length > 2 ) {
            return d[2] + '. ' + d[1] + '. ' + d[0];
        }
    },

    dateTimeBorn: {},


    param2json: function(s) {
      var result = {};
      s.replace(/\+/g, '%20').split("&").forEach(function(part) {
        var item = part.split("=");
        var val = item[1];
        try {
          val = decodeURIComponent(val);
        } catch (e) {
        }

        if ( result[item[0]] ) {
          result[item[0]] = result[item[0]] + ', ' + val;
        } else {
          result[item[0]] = val;
        }
      });
      return result;
    },


    post : null,

    formState: function( f$, action ) {
      if ( action === 'save' ) {
        //console.log('save ' + f$.attr('id') );
        var state = '';
        f$.find('input, select').each(function() {
          if ( this.type === 'radio' || this.type === 'checkbox' ) {
            if (this.checked) {
              state += this.name + '\u000c' + this.value + '\u000c'
            }
          } else {
            state += this.name + '\u000c' + this.value + '\u000c'
          }
        });
        Cookies.set("vb-frm-" + f$.attr('id'), state, { expires: 1 });
      } else if ( action === 'restore' ) {
        var state = Cookies.get("vb-frm-" + f$.attr('id'));
        if ( state && state.length && state.length > 1 ) {
          var fields = (state).split('\u000c');
          for(var i=0; i<fields.length; i+=2) {
            //console.log('restore ' + f$.attr('id'), controlName, controlValue);
            var controlName = fields[i];
            if ( controlName ) {
              var controlValue = fields[i+1];
              // checkers...
              var c$ = f$.find('[name='+ controlName +']');
              if ( c$.attr('type') === 'radio' || c$.attr('type') === 'checkbox' ) {
                f$.find('[name='+ controlName +'][value=' + controlValue + ']').attr('checked', true);
              } else {
                c$.val(controlValue);
              }
            }
          }
          // select special
          // f$.find('select').trigger('change').selectpicker('refresh');
        }
      }
    },



    validator: {
      emsho: function(value) {
        if (!/^\d{13}$/.test(value)) {
            return false;
        }
        var day   = parseInt(value.substr(0, 2), 10),
            month = parseInt(value.substr(2, 2), 10),
            year  = parseInt(value.substr(4, 3), 10),
            rr    = parseInt(value.substr(7, 2), 10),
            k     = parseInt(value.substr(12, 1), 10);

        // Validate date of birth
        // FIXME: Validate the year of birth
        if (day > 31 || month > 12) {
            return false;
        }

        // Validate checksum
        var sum = 0;
        for (var i = 0; i < 6; i++) {
            sum += (7 - i) * (parseInt(value.charAt(i), 10) + parseInt(value.charAt(i + 6), 10));
        }
        sum = 11 - sum % 11;
        if (sum === 10 || sum === 11) {
            sum = 0;
        }
        if (sum !== k) {
            return false;
        }

        return true;
      }
    },



    frmWaitTimer: null,
    frmWaitInt: 500,
    frmActionOne: true, // on 2nd - false,
    frmWaitProgress: 5,
    frmWait: function (s, t) {
        if (s === null) {
            // default show - progress indicator
            $('#frmWait .modal-footer button').hide();
            $('#frmWait .modal-header .modal-title').html('<i class="fa fa-lg fa-clock-o"></i> ' + tSite.lang['one-moment'] + '...');
            $('#frmWait .modal-body').html(
              '<div class="progress">'
                  + '<div class="progress-bar progress-bar-primary progress-bar-striped active" '
                  + 'style="width: ' + tSite.frmWaitProgress + '%">'
                  + '</div>'
              + '</div>'
            );
            tSite.frmWaitTimer = setInterval((function () { tSite.frmWait('+'); }), 500);
            $('#frmWait').modal('show');

        } else if (s === '+') {
            var $t = $('#frmWait .modal-body div.progress div.progress-bar');
            tSite.frmWaitProgress += 5;
            if (tSite.frmWaitProgress > 100) { tSite.frmWaitProgress = 5; }
            $t.css('width', tSite.frmWaitProgress + '%');
            return true;

        } else if (s === 'hide') {
            // hide
            clearInterval(tSite.frmWaitTimer);
            tSite.frmWaitTimer = null; // to enable helper
            tSite.frmWaitProgress = 5;
            $('#frmWait').modal('hide');

        } else {
            // show message
            clearInterval(tSite.frmWaitTimer);
            tSite.helperCounter = 0; // reset helper counter
            tSite.frmWaitTimer = null; // to enable helper
            var tit = (t || tSite.lang['message']);
            $('#frmWait .modal-body').html(s);
            $('#frmWait .modal-header .modal-title').html(tit);
            $('#frmWait .modal-footer button').show();
            $('#frmWait').modal('show');

        }
    },



    // jivo || zopim chat
    openChat: function () {

      if (typeof (jivo_api) !== 'undefined') {
        jivo_api.open();
        // console.log( $('#jivo-label-text').offset() );
        return true;
      }

      if (typeof($zopim) !== 'undefined') {
        $zopim(function() {
          $zopim.livechat.window.show();
        });
        return true;
      }

      return false;
    },



    // kukiman
    cookieBanner: null,
    cookieNotice: function () {
      if ( tSite.cookieBanner !== null ) {
        if (Cookies.get("cookieu") !== "ok") {
          $("body").append( $(
            '<div class="alert alert-info text-center fade in" '
              //+ 'role="alert" style="position: fixed; bottom: 0; width: 100%; '
              + 'role="alert" style="position: fixed; bottom: 0; left: 0; right: 0; '
              + 'margin-bottom: 0; padding-bottom: 1em; z-index:10; color: white;">'
              + tSite.cookieBanner
            + '</div>' ) );
        }
      }
    },
    cookieNoticeOff: function () {
        Cookies.set("cookieu", "ok", { expires: 365 });
    },



    printElement: function(elem) {

        var popwin = function(data) {

            var w = window.open('', 'my div', 'height=' + Math.floor(screen.height*.8) + ',width=' + Math.floor(screen.width*.8) );
            w.document.write('<html><head><title>Print</title>');
            w.document.write('<style> table { font-family:sans-serif; font-size:9pt;} </style>');

            /*optional stylesheet*/
            // w.document.write('<link rel="stylesheet" href="css/webshop.css" type="text/css" />');

            w.document.write('</head><body><main><div id="vb-printable">');
            w.document.write(data);
            w.document.write('</div></main></body></html>');

            w.document.close(); // necessary for IE >= 10
            w.focus(); // necessary for IE >= 10

            w.print();
            w.close();

            return true;
        }

        popwin($(elem).html());

    },

    sizeIframe: function() {
      //TODO
    },


    chartise: function() {

      function draw() {

        var options = {
          title: null,
          legend: 'none',
          chartArea: { left:'20%', top:'0%', width:'90%', height:'90%' },
          pieHole: 0.5,
          colors: tSite.colors,
          slices: { 2:{textStyle: {color: 'black' }} },
          backgroundColor: 'transparent',
          fontName: 'Open Sans',
          fontSize: 14,
          tooltip: {
            textStyle: {fontSize: 14}
          },
          _last: false
        };
        var numFormatter = new google.visualization.NumberFormat({ pattern: '#,##0.00' });
        var cnt = 0;

        c.each(function() {
          var e = $(this);
          var tc = e.closest('div.table-responsive');
          var c = $(
            '<div class="row">'
              + '<div class="col-md-6">'
                + '<div id="chart-table-' + cnt + '"></div>'
              + '</div>'
              + '<div class="col-md-6">'
                + '<div id="donut-chart-' + cnt + '" style="height:320px;"></div>'
              + '<div>'
            + '<div>'
          );

          c.insertBefore(tc);
          tc.appendTo('div#chart-table-' + cnt);

          var data = new google.visualization.DataTable();
          var i = 0;
          e.find('tr').each(function() {
              if ( i === 0 ) {
                data.addColumn('string', $(this).find('td:eq(0)').text() );
                data.addColumn('number', $(this).find('td:eq(1)').text() );
              } else {
                data.addRow([
                  $(this).find('td:eq(0)').text(),
                  parseFloat( $(this).find('td:eq(1)').text().replace( /,/, '.' ) )
                ]);
              }
              if (++i > 4) return false;
          });
          numFormatter.format(data, 1);

          var chart = new google.visualization.PieChart(document.getElementById('donut-chart-' + cnt));
          chart.draw(data, options);

          cnt++;

        }); // each()

      } // draw()

      var c = $('table[data-chart-type="pie"]');
      if ( c.length > 0 ) {

        google.charts.load("current", {packages:["corechart"], language:'sl'});
        google.charts.setOnLoadCallback(draw);

      } // if()

    },


    webshop: {
        authURL: ''
    },

    travel: {
        pmax: 5,
        minDay: null,
        maxDay: null,
        checkAge: function(value, validator) {
            //IMPORTANT NOTICE: You have to declare the callback as a global function outside of $(document).ready()
            //DEBUG: console.log( tSite.travel.minDay.format(tSite.DATE_FORMAT), tSite.travel.maxDay.format(tSite.DATE_FORMAT) );
            var m = new moment( value, tSite.DATE_FORMAT );
            if ( m.isValid() ) {
                var y = tSite.travel.maxDay.diff(m, 'years', false); // true for year fraction as well
                if ( y > 75 ) {
                    return {
                        valid: false,
                        message: 'Не сме да се изда полиса за лица стария од 75 година'
                    };
                } else if ( y > 70 && (tSite.post.pers > 1 || tSite.post.type !== 'i') ) {
                    return {
                        valid: false,
                        message: tSite.lang['travel-multi-71']
                    }
                } else if ( y > 70 && tSite.post.days > 30 ) {
                    return {
                        valid: false,
                        message: 'Не сме да се изда полиса за лица стария од 70 година, за период дужи од 30 дана'
                    }
                } else {
                    return true;
                }
            } else {
                return {
                    valid: false,
                    message: 'Внесете валиден датум'
                }
            }
        }
    },

    scrollTop: function() {
      $('html').animate({scrollTop:0}, 100);
      $('body').animate({scrollTop:0}, 100);
    },

    _last: false

};

// scroll top (for FF)
$(document).ready(function(){
  tSite.scrollTop();
});


// DOM ready
$(function() {

    // init
    tSite.w$ = $(window);


    // depo
    // -A
    $('a[href*="/media/store/"]').each(function() {
      var t$ = $(this);
      var h = t$.attr('href');
      var i = h.indexOf('/media/store/');
      t$.attr('href', t$.attr('href').slice(i));
      if ( h.endsWith('.pdf') || h.endsWith('.doc') || h.endsWith('.jpg') || h.endsWith('.png') ) {
        t$.attr('target', '_blank');
      }
    });


    // resp. table
    $('.main-content table.table').wrap( $('<div class="table-responsive" />') );

    // table charts
    tSite.chartise();



    // slider finale
    $('.flexslider li[data-slidr-slide]').each(function() {
      var id = $(this).data('slidr-slide');
      var bg = $(this).find('div div[style]').css('background-image');
      $('.slidr-btn-new.item-' + id).css('background-image', bg);
    });

    $('.s-tiles').flexslider({
      animation: 'fade',          //String: Select your animation type, "fade" or "slide"
      easing: 'swing',            //String: Determines the easing method used in jQuery transitions. jQuery easing plugin is supported!
      directionNav: false,        //Boolean: Create navigation for previous/next navigation? (true/false)
      fadeFirstSlide: false,
      // controlNav: 'thumbnails',
      manualControls: "#slider-navigation .slidr-btn",
      //useCSS: false,
      pauseOnHover: true,

      slideshowSpeed: 5000,
      animationSpeed: 500,
      controlNav: false,

      start: function(slider) {
        // slider.removeClass('loading');
        if ( tSite.ln === 'hr' ) {
          // 1st show
          slider.pause();
          setTimeout(function() {
              slider.play();
          }, 10000);
        }
      },
      after: function(slider) {
        if ( tSite.ln === 'hr' ) {
          // 2nd and later show
          if(slider.currentSlide == 0) {
              slider.pause();
              setTimeout(function() {
                  slider.play();
              }, 5000);
          }
        }
      }

    });

    $('.s-stock').flexslider({
      animation: 'fade',          //String: Select your animation type, "fade" or "slide"
      easing: 'swing',            //String: Determines the easing method used in jQuery transitions. jQuery easing plugin is supported!
      directionNav: true,         //Boolean: Create navigation for previous/next navigation? (true/false)
      fadeFirstSlide: false,
      //controlNav: 'thumbnails',
      //manualControls: "#slider-navigation .slidr-btn",
      //useCSS: false,
      pauseOnHover: true
    });


    // navigate on click for some url
    $('.sl-si .s-stock ul.slides > li[data-slidr-submit-url]').on('click', function(e) {
      var url = $(this).data('slidrSubmitUrl');
      if ( url.slice(0,4) === 'http' ) {
        window.location.assign( $(this).data('slidrSubmitUrl') );
      }
    });


    // hr hover change
    $(".hr-hr .slider-nav-btn, .savaos.sr-rs .slider-nav-btn").hover(
        function() { $('.flexslider').flexslider($(this).index()); },
        function() {}
    );

    // hr slidr tile clicks
    $('.hr-hr .slider-nav-btn > div[data-slidr-target], .savaos.sr-rs .slider-nav-btn > div[data-slidr-target]').on('click', function() {
      $('[data-slidr-slide="' + $(this).data('slidrTarget') + '"]').click();
    });
    // navigate on click
    $('.hr-hr .s-tiles ul.slides > li[data-slidr-submit-url], .savaos.sr-rs .s-tiles ul.slides > li[data-slidr-submit-url]').on('click', function(e) {
      if ( e.target.tagName === 'LI' || e.target.tagName === 'P' || e.target.tagName === 'H1' ||
        e.target.tagName === 'H2' || e.target.tagName === 'A' || e.target.className === 'slidr-slide' ) {
        window.location.assign( $(this).data('slidrSubmitUrl') );
      }
    });


    // me/mk/sr/sq slidr tile click
    //if ( tSite.ln === 'sr' || tSite.ln === 'mk' || tSite.ln === 'me' || tSite.ln === 'sq' ) {
    if ( tSite.ln === 'mk' || tSite.ln === 'me' || tSite.ln === 'sq' ) {
      $('.slider-nav-btn > div[data-slidr-target]').on('click', function() {
        $('.s-tiles').flexslider( $(this).data('slidrNum') );
      });
      $('ul.slides li[data-slidr-submit-url]').on('click', function(e) {
        if ( e.target.className === '' ) {
          window.location.assign( $(this).data('slidrSubmitUrl') );
        }
      });
    }


    // button url
    $('button[data-href]').on('click', function() {
      window.location.assign( $(this).data('href') );
    });


    // datepickers
    tSite.dateTimeBorn = {
      locale: tSite.ln,
      format: tSite.DATE_FORMAT,
      viewMode: 'years',
      viewDate: moment().subtract(19, 'y'),
      useCurrent: false,
      minDate: tSite.bornDate.min,
      maxDate: tSite.bornDate.max
    };
    $('.date-born').datetimepicker( tSite.dateTimeBorn );

    $('.date-future').each(function() {
        var dmax = $(this).data('future');
        $(this).datetimepicker({
          locale: tSite.ln,
          format: tSite.DATE_FORMAT,
          minDate: tSite.now.clone().add(1, 'd'),
          maxDate: tSite.now.clone().add(dmax, 'd')
        })
    });

    $('.date-past').each(function() {
        var dmin = $(this).data('past');
        $(this).datetimepicker({
          locale: tSite.ln,
          format: tSite.DATE_FORMAT,
          minDate: tSite.now.clone().add(-dmin, 'd'),
          maxDate: tSite.now.clone()
        })
    });

    $('.date').on('dp.change dp.show', function (e) {
        if ( e.date ) {
            var fld = $(e.target).find('input').attr('name');
            var frm = $(e.target).closest('form');
            if ( frm && fld ) frm.formValidation('revalidateField', fld);
        }
    });

    // typeahead
    // - autocomplete city + postal
    if ( tSite.ln === 'mk' ) {
      $('.input-city')
          .typeahead({source:MKD.p, items:8, minLength:2})
          .blur(function() {
              if(MKD.p.indexOf($(this).val()) === -1)
                  $(this).val('');
              }
          );
    }



    // menu helpers
    // - height helper
    var resizeTimer;
    var resizeHelper = function() {
      if ( $(window).width() < 768 ) {
        $('.my-dropdown-big').css({
          'height': 'auto',
          'overflow-y': 'hidden'
        });
      } else {
        $('.my-dropdown-big').css({
          'height': ( $(window).height() - 120 ) + 'px',
          'overflow-y': 'auto'
        });
      }
    }
    // - on resize(end)
    $(window).on('resize', function(e) {
      clearTimeout(resizeTimer);
      resizeTimer = setTimeout(resizeHelper, 350);
    });
    // - initial
    resizeHelper();
    
    // - group toggle
    $('ul.dropdown-menu [data-toggle=dropdown]').on('click', function(e) {
        e.preventDefault();
        e.stopPropagation();

        var t$ = $(this);

        var m$ = t$.parent().find('ul');

        // slideup all other open
        $('ul.my-dropdown-big ul.so-dropdown-submenu').each(function() {
            if ( $(this).attr('id') !== m$.attr('id') ) {
                $(this).slideUp();
                $(this).parent().find('a').removeClass('dropped');
            }
        });

        // toggle submenu
        m$.slideToggle(function() {
          t$.toggleClass('dropped');
        });
        // $('#auto-submenu').slideToggle();

        //$(this).parent().siblings().removeClass('open');
        //$(this).parent().toggleClass('open');

        resizeHelper();

    });

    // - close menu
    var dropCloseTimer = null;
    var dropClose = function() {
        $('.dropdown.open .dropdown-toggle').dropdown('toggle');
        $('ul.dropdown-menu ul.so-dropdown-submenu').hide();
        $('ul.dropdown-menu [data-toggle="dropdown"]').removeClass('dropped');
    }
    $('ul.my-dropdown-big').hover(
        function() {
            clearTimeout(dropCloseTimer);
        },
        function() {
            //console.log('Maus OUT', new Date());
            //dropCloseTimer = setTimeout( dropClose, 500 );
            if ( $('#vguard').is(':visible') ) {} else { dropClose(); }
        }
    );


    // search trigger
    $('a#a-404-search').on('click', function(e) {
      e.preventDefault();
      $('#frm-search').closest('li.dropdown').find('a').click();
      $('#frm-search input[name=q]').focus();
      return false;
    });


    /* accordion  bug.
    $(document).on('shown.bs.collapse', function() {
        $('.collapse > .nav > .xs-expand').addClass('open');
    });
    */


    // news flash
    /*
    if ( $('#news-depo').length > 0 ) {
      var news = (function() {
          var i = 0;
          var s = '';
          //$('#news-depo .container .news .media').each(function() {
          var ln = $('#news-depo').data('lang');
          $('#news-depo .media').each(function() {
              var n$ = $(this).clone();
              var id = n$.attr('id');
              n$.find('div.collapse').remove();
              n$.find('p').wrapAll( $('<a href="' + ln + '#' + id + '"/>') );
              s += '<div class="media">' + n$.html() + '</div>';
              if ( ++i > 2 ) {
                  return false;
              }
          });
          return s;
      })();

      news = news.replace(/ data-dbe-/gi, 'x-');
      var newsToc$ = $('#news-toc');
      if ( newsToc$.length > 0 ) {
          var ln = $('#news-depo').data('lang');
          newsToc$.append(
              '<div class="row"><div class="col-md-12 news">' + news
              + '<p><a href="' + ln + '">' + tSite.lang['more-news'] + '</a></p></div></div>'
          );
      }
      var newsCol$ = $('#news-col');
      if ( newsCol$.length > 0 ) {
          var ln = $('#news-depo').data('lang');
          newsCol$.append(
              '<p class="h2">' + tSite.lang['news'] + '</p>' + news.replace(/class="h2"/gi, 'class="h3"')
              + '<p><a href="' + ln + '">' + tSite.lang['more-news'] + '</a></p>'
          );
      }
    }
    */


    // blog
    var blogToc$ = $('#blog-toc');
    if ( blogToc$.length > 0 ) {
      $.ajax({
        url: '/Content/SavaTemplate/comp/proxy/',
        method: 'GET',
        type: 'xml',
        data: '__proxy_url=' + encodeURIComponent( blogToc$.data('feed'))
      }).always(function(o, status, rq) {
        if ( status === 'success' ) {
          var x$ = $($.parseXML(o));
          var count = 0;
          var blog = '';
          x$.find('item').each(function() {
            var $this = $(this),
              item = {
                  title: $this.find('title').text(),
                  link: $this.find('link').text(),
                  description: $this.find('description').text(),
                  pubDate: new Date($this.find('pubDate').text()),
                  author: $this.find('author').text()
              };
            blog += '<div class="media"><div class="media-body">'
              + '<a href="' + item.link + '" target="_blank">'
              + '<span class="date">' + moment(item.pubDate).format(tSite.DATE_FORMAT) + '</span>'
              + '<span class="h2">' +item.title + '</span></a>'
              + '</div></div>';
            // up to 3
            if ( count++ > 1 ) { return false; }
          });
          blogToc$.html(
            '<div class="row"><div class="col-md-12 news">' + blog + '</div></div>'
          );
        }
      });
    }


    // contact preselect?
    var m = document.location.search.match(/c=([^&]+)/i);
    if (m && m.length && m.length > 1 ) {
       $('select#who option[value="' + m[1] + '"]').prop('selected', true);
    }



    // FORM Restore
    tSite.formState($('form.form-stateful:first'), 'restore');




    // CHAT
    $('a#a-chatpop, a.chatpop').on('click', function(e) {
      e.preventDefault();
      tSite.openChat();
      return false;
    });


    // CONTACT
    // - reset form on back/forward
    $(window).bind("pageshow", function() {
      if ( $('form#frm-contact').length > 0 ) $('form#frm-contact')[0].reset();
    });
    // - form validation
    $('form#frm-contact').formValidation({
        framework: 'bootstrap',
        locale: tSite.lang['l-code2'],
        _last: false
    });
    $('form#frm-contact #type-sr').on('change', function() {
      if ( $(this).find(':selected').data('upload') === true ) {
        $('#div-f-upload').show();
        $('#sr-file-cv').prop('disabled', false);
        document.getElementById('frm-contact').enctype='multipart/form-data';
      } else {
        $('#div-f-upload').hide();
        $('#sr-file-cv').prop('disabled', true);
        document.getElementById('frm-contact').enctype='application/x-www-form-urlencoded';
      }
    });
    $('form#frm-contact #btn-contact').on('click', function() {
        var fv = $('form#frm-contact').data('formValidation');
        fv.resetForm(false); // valid date cleared is not detected
        fv.validate();
        if ( !fv.isValid() ) {
            fv.getInvalidFields().first().focus();
        } else {
            // captcha
            // $('#frm-contact [name="g-recaptcha-response"]').val()
            var grc = grecaptcha.getResponse();
            if ( grc.length === 0 ) {
              $('#div-g-recaptcha').addClass('has-error');
              // grecaptcha fail
              return;
            } else {
              // OK
            }

            $('form#frm-contact #btn-contact').prop('disabled', true);

            if ( document.getElementById('frm-contact').enctype === 'multipart/form-data' ) {
              // file upload
              document.getElementById('frm-contact').submit(); // IE8+

            } else {
              // contact ajax
              $.ajax({
                  url: './',
                  method: 'POST',
                  type: 'json',
                  data: 'cmd=contact-me&' + $('#frm-contact').serialize()
              }).always(function(o, status, rq) {
                  if ( status === 'success' ) {
                      $('form#frm-contact').fadeOut(function() {
                          $(this).parent().append('<p>' + o.msg + '</p>');
                      });
                  } else {
                      //DEBUG:
                      console.log(status, o);
                  }
              });

              tSite.scrollTop();
              return false;

            }

        } // isValid

    }); // btn click



    // Pritožbe
    // - type radios
    $('#frm-complaint input[name="pravni"]').on('change', function(){
      if ($("input[name='pravni']:checked").attr('id') === 'c-lice-p') {
        $('.pravno').show();
        $('.fizicko').hide();
      } else {
        $('.fizicko').show();
        $('.pravno').hide();
      }
    });
    // - validation
    $('#frm-complaint').formValidation({
      framework: 'bootstrap',
      locale: tSite.lang['l-code2'],
      _last: false
    });
    // - submit
    $('form#frm-complaint #btn-complaint').on('click', function() {
      var fv = $('form#frm-complaint').data('formValidation');
      fv.resetForm(false); // valid date cleared is not detected
      fv.validate();
      if ( !fv.isValid() ) {
        fv.getInvalidFields().first().focus();
      } else {
        var grc = grecaptcha.getResponse();
        if ( grc.length === 0 ) {
          $('#div-g-recaptcha').addClass('has-error');
          // grecaptcha fail
          // return;
        } else {
          // OK
        }

        $('form#frm-complaint #btn-complaint').prop('disabled', true);

        var s = '<b>Podnošenje prigovora</b><br><table>'
            + '<tr><td colspan="2">'
            + $('form#frm-complaint').find('input[type="radio"]:checked').val()
            + '</tr>';

        $('form#frm-complaint').find('input,textarea').each(function() {
          if ( this.type === 'radio' ) {
            // done above
          } else {
            // text fields
            var l = $(this).parent().find('label > span:visible').text();
            if ( l.length > 1 ) {
              s+= '<tr><td>' + l + '<td>' + $(this).val() + '</tr>';
            }
          }
        });

        s += '</table>';

        // contact ajax
        $.ajax({
            url: './',
            method: 'POST',
            type: 'json',
            data: 'cmd=contact-me&m-type=complaint&m-content=' + encodeURIComponent(s)
        }).always(function(o, status, rq) {
            if ( status === 'success' ) {
                $('form#frm-complaint').fadeOut(function() {
                    $(this).parent().append('<p>' + o.msg + '</p>');
                });
            } else {
                //DEBUG:
                console.log(status, o);
            }
        });

        tSite.scrollTop();
        return false;

      }
    });



    // PAGE HELPERS
    // - menu
    var menu = $('#nav-so-menu').clone();
    menu.find('li > a[href="' + document.location.pathname + '"]').parent().addClass('active');
    $('ul#so-menu-company').append(menu.html());

    // - submenu
    var ali$ = $('.main-content ul.so-menu li a[href="' + document.location.pathname + '"]');
    ali$.parent().addClass('active');
    ali$.parent().parent().closest('li').addClass('active');

    // - embed menu
    $('ul.so-sub-menu > li > a').on('click', function(e) {
      var t$ = $(this);
      if ( t$.attr('href').length > 5 ) {
        // go
        return true;
      } else {
        e.preventDefault();
        $(this).closest('ul').find('li').removeClass('active');
        $(this).parent().addClass('active');
        return false;
      }
    });

    // - special styling
    $('div.so-documents ul > li > a').each(function() {
        var t$ = $(this);
        var h = t$.attr('href');
        if ( h.slice(-4) === '.pdf' ) {
            t$.parent().addClass('pdf');
        }
    });


    // CHAT
    // - Status
    if ( typeof($zopim) === 'function' ) {
      $zopim(function() {
        $zopim.livechat.set({
          onStatus: function(s) {
            $('.shortcuts .indicator').addClass('hidden');
            if (s == 'online' ) {
              $('.shortcuts .chat-online').removeClass('hidden');
            } else {
              $('.shortcuts .chat-offline').removeClass('hidden');
            }
          }
        });
      });
    }

    $('a.a-zopim').on('click', function(e) {
      e.preventDefault()
      tSite.openChat();
      return false;
    });


    // CONTENT TRIGGERS
    /*
    $('div.main-content p:contains(#TMD_)').each(function() {
        // -search inset
        if ($(this).text() === '#TMD_SEARCH_HITS#') {
        }
    });
    */




    // open/close
    $('.news h2, .more').on('click', function() {
        $(this).next('div').toggleClass('vis');
    });

    $('.news > .media > .media-body > .h2').on('click', function() {
        $(this).find('~ div.collapse').stop().slideToggle({
            duration: 250,
            easing: 'easeInSine',
        });
    });
    if ( document.location.hash.slice(0,6) === '#news-' ) {
        var offset = $('.media' + document.location.hash + ' .h2').click().offset();
        $('html, body').animate({
            scrollTop: offset.top - 120,
            scrollLeft: offset.left
        });
    }


    // Expand/collapse
    // - add button
    if ( $('main p > a.expand-collapse').length > 0 ) {
    } else {
      var exp = $('main div.expandable');
      if ( exp.length > 0 ) {
        var id = exp.attr('id');
        if ( typeof(id) === 'undefined' ) {
          id = 'colexp' + Math.floor(Math.random()*1000);
          exp.attr('id', id);
        }
        $('<p><a class="expand-collapse" href="#" data-div="' + id + '">' + tSite.lang['more'] + '</a></p>').insertAfter( exp );
      }
    }
    // - on click
    $('a.expand-collapse').on('click', function(event){
        event.preventDefault();
        var $t = $(this);
        var id = $t.data('div');
        $('#' + id).slideToggle({
          duration: 250,
          easing: 'easeInSine',
          complete: function() { $t.hide(); }
        });

        /*
        $('#' + id).animate({
          height: 700
        }, function(){
            $(this).css('height', 'auto')
        });*/
        //var t = $t.data('off');
        //$t.data('off', $t.text()).toggleClass('chevron-up').text(t);
        return false;
    });

    // Edit
    $('span.edit a[title=Close]').on('click', function() { $('span.edit, .edit-tool').hide() });


});



// Can also be used with $(document).ready()
/*
$(window).load(function() {
});
*/

