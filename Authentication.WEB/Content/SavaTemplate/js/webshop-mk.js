'use strict';

// DOM ready
$(function() {

    // Travel

    // - init state
    if ( tSite.post['date-start'] && tSite.post['date-end']) {
        tSite.travel.minDay = moment(tSite.post['date-start'], tSite.DATE_FORMAT)
        tSite.travel.maxDay = moment(tSite.post['date-end'], tSite.DATE_FORMAT)
    } else if ( tSite.post['days'] ) {
        tSite.travel.minDay = tSite.now.clone().add(1,'days');
        tSite.travel.maxDay = tSite.now.clone().add(tSite.post['days'],'days');
    } else {
        tSite.travel.minDay = tSite.now.clone().add(1,'days');
        tSite.travel.maxDay = tSite.now.clone().add(3,'days');
    }
    $('.so-travel form#frm-so-calc #travel-type').val(tSite.post['type']);
    $('.so-travel form#frm-so-calc #travel-pers').val(tSite.post['pers']);
    $('.so-travel form#frm-so-calc #travel-date-s').val(tSite.travel.minDay.format(tSite.DATE_FORMAT));
    $('.so-travel form#frm-so-calc #travel-date-e').val(tSite.travel.maxDay.format(tSite.DATE_FORMAT));


    // - types / persons sync
    $('#travel-type').on('change', function() {
        if ( $(this).val() === '5' ) {
            $('#travel-pers').val('5');
        } else if ( $(this).val() === '2' ) {
            $('#travel-pers').val('2');
        } else if ( $(this).val() === '1' ) {
            $('#travel-pers').val('1');
        }
    });
    $('#travel-pers').on('change', function() {
        var n = parseInt( $(this).val(), 10 );
        if ( isNaN(n) ) n = 1;
        var t = $('#travel-type').val();
        if ( n === 1 && ( t === '2' || t === '5' ) ) {
            $('#travel-type').val('1');
        } else if ( n < 5 && t === '5' ) {
            $('#travel-type').val('2');
        } else if ( n > 4 && ( t === '1' || t === '2' ) ) {
            $('#travel-type').val('5');
        } else if ( t === '1' ) {
            $('#travel-type').val('2');
        }
    });


    // ugovaratelj
    $('#cb-contract').on('click', function() {
        if ( $(this).prop('checked') ) {
            $('#div-travel-p-u, #div-home-p-u').hide();
            $('#div-travel-p-u input, #div-home-p-u input').prop('disabled', true);
        } else {
            $('#div-travel-p-u input, #div-home-p-u input').prop('disabled', false);
            $('#div-travel-p-u, #div-home-p-u').show();
        }
    });

    // add/rmv person
    $('#btn-travel-add-p, #btn-travel-remove-p').on('click', function(e) {
        e.preventDefault();
        var n = $(this).data('pnum');
        $('#frm-so-calc input[name=pers]').val(n);
        tSite.formState($('form.form-stateful:first'), 'save');
        $('#frm-so-calc').submit();
        return false;
    });

    /*
    $('#btn-travel-add-p').on('click', function() {
        var n = $('#div-travel-p-' + tSite.travel.pmax).html();
        var rx = new RegExp('="pers\-([a-z]+)\-' + tSite.travel.pmax + '"', 'gi');
        n = n
                .replace(rx, '="pers-$1-' + (tSite.travel.pmax+1) + '"')
                .replace(/Осигурано лице #[0-9]+/, 'Осигурано лице #' + (tSite.travel.pmax+1));
        $('#div-travel-p-' + tSite.travel.pmax).parent().append( $('<div id="div-travel-p-' + (tSite.travel.pmax+1) + '">' + n + '</div>') );
        tSite.travel.pmax++;
        $('#btn-travel-remove-p').show();
    });
    // remove person
    $('#btn-travel-remove-p').on('click', function() {
        $('#div-travel-p-' + tSite.travel.pmax).remove();
        tSite.travel.pmax--;
        if ( tSite.travel.pmax == 5 ) {
            $(this).hide();
        }
    });
    */



    // - pers details copy
    $.map( ['pers-lname', 'pers-street', 'pers-postno', 'pers-city' ], function(val, i) {
        $('input[name^="' + val + '-"]').on('change', function() {
            var t = this;
            $('input[name^="' + val + '-"]').each(function() {
                if ( this !== t && this.value === '' ) {
                    this.value = t.value;
                    $(this.form).formValidation('revalidateField', this.name);
                }
            });
        });
    });
    // - person travel age...
    $('.so-travel input[name^="pers-bdate-"]').on('blur', function() {
        //lbl-pers-bdate-
        var m = new moment( $(this).val(), tSite.DATE_FORMAT );
        if ( m.isValid() ) {
            $('#lbl-' + this.name).html('Возраст за време на патувањето: <strong>' + tSite.travel.maxDay.diff(m, 'years', false) + 'г.</strong>');
        } else {
            $('#lbl-' + this.name).html('');
        }
    });
    // - person EMBG.>bdate
    $('input[name^="pers-embg-"]').on('blur', function() {
        var t$ = $(this);
        var f$ = $('input[name="' + t$.attr('name').replace(/pers\-embg\-/, "pers-bdate-") + '"]');
        var v = t$.val();
        if ( f$.length > 0 && f$.val().length < 1 && v.length > 6) {
            var m = moment( v.slice(0,2)+'.'+v.slice(2,4)+'.'+(v.slice(4,5)==='9'?'1':'2')+v.slice(4,7), tSite.DATE_FORMAT);
            if ( m.isValid() && m.isBefore( tSite.now ) ) f$.val( m.format(tSite.DATE_FORMAT) );
        }
    });
    // - home package
    $('.so-home-package input[type=radio]').on('click', function() {
      var v = parseInt( $(this).data('val'), 10 );
      if ( !isNaN(v) ) {
        $('#insu-sum').html('Домаќинско осигурување <span>' + tSite.formatNumber(v, 2) + ' ден</span>');
        $('#calc-sum').html('Домаќинско осигурување <span>' + tSite.formatNumber(v, 2) + ' ден</span>');
      }
    });

    // - enable btn
    $('#frm-so-calc select, #frm-so-calc input').on('change keypress', function() {
        $('#insu-sum').fadeTo('def', 0);
        $('#btn-recalc').fadeTo('def', 1);
        return true;
    });
    // - enable btn, date validate / chain update
    $('#frm-so-calc .date-future').on('dp.change', function() {
        var i$ = $(this).find('input');

        var d1 = moment( $('#travel-date-s').val(), tSite.DATE_FORMAT);
        var d2 = moment( $('#travel-date-e').val(), tSite.DATE_FORMAT);

        if ( !d1.isValid() || d1.isBefore(tSite.now) ) {
            d1 = tSite.now.clone().add(1, 'days');
            $('#travel-date-s').val( d1.format(tSite.DATE_FORMAT) );
        }
        if ( !d2.isValid() ) {
            d2 = tSite.now.clone().add(tSite.post['days']-1, 'days');
            $('#travel-date-e').val( d2.format(tSite.DATE_FORMAT) );
        }

        if ( i$.attr('id') === 'travel-date-s' ) {
            $('#travel-date-e').val( d1.add(tSite.post['days']-1,'days').format(tSite.DATE_FORMAT) );
        } else if ( i$.attr('id') === 'travel-date-e' ) {
            var df = d2.diff(d1, 'days', false)+1;
            if ( isNaN(df) ) df = 0;
            if ( df > 90 ) {
                df = 90;
                tSite.post['days'] = df;
                i$.val( d1.add(tSite.post['days']-1,'days').format(tSite.DATE_FORMAT) );
            } else if ( df > 0 ) {
                tSite.post['days'] = df;
            } else {
                i$.val( d1.add(tSite.post['days']-1,'days').format(tSite.DATE_FORMAT) );
            }
        }

        $('#insu-sum').fadeTo('def', 0);
        $('#btn-recalc').fadeTo('def', 1);
        return true;
    });


    // - init state
    if ( tSite.post['date-start'] && tSite.post['date-end']) {
        tSite.travel.minDay = moment(tSite.post['date-start'], tSite.DATE_FORMAT)
        tSite.travel.maxDay = moment(tSite.post['date-end'], tSite.DATE_FORMAT)
    } else if ( tSite.post['days'] ) {
        tSite.travel.minDay = tSite.now.clone().add(1,'days');
        tSite.travel.maxDay = tSite.now.clone().add(tSite.post['days'],'days');
    } else {
        tSite.travel.minDay = tSite.now.clone().add(1,'days');
        tSite.travel.maxDay = tSite.now.clone().add(3,'days');
    }
    $('.so-travel form#frm-so-calc #travel-type').val(tSite.post['type']);
    $('.so-travel form#frm-so-calc #travel-pers').val(tSite.post['pers']);
    $('.so-travel form#frm-so-calc #travel-date-s').val(tSite.travel.minDay.format(tSite.DATE_FORMAT));
    $('.so-travel form#frm-so-calc #travel-date-e').val(tSite.travel.maxDay.format(tSite.DATE_FORMAT));


    // - types / persons sync
    $('#travel-type').on('change', function() {
        if ( $(this).val() === '5' ) {
            $('#travel-pers').val('5');
        } else if ( $(this).val() === '2' ) {
            $('#travel-pers').val('2');
        } else if ( $(this).val() === '1' ) {
            $('#travel-pers').val('1');
        }
    });
    $('#travel-pers').on('change', function() {
        var n = parseInt( $(this).val(), 10 );
        if ( isNaN(n) ) n = 1;
        var t = $('#travel-type').val();
        if ( n === 1 && ( t === '2' || t === '5' ) ) {
            $('#travel-type').val('1');
        } else if ( n < 5 && t === '5' ) {
            $('#travel-type').val('2');
        } else if ( n > 4 && ( t === '1' || t === '2' ) ) {
            $('#travel-type').val('5');
        } else if ( t === '1' ) {
            $('#travel-type').val('2');
        }
    });


    // ugovaratelj
    $('#cb-contract').on('click', function() {
        if ( $(this).prop('checked') ) {
            $('#div-travel-p-u, #div-home-p-u').hide();
            $('#div-travel-p-u input, #div-home-p-u input').prop('disabled', true);
        } else {
            $('#div-travel-p-u input, #div-home-p-u input').prop('disabled', false);
            $('#div-travel-p-u, #div-home-p-u').show();
        }
    });

    // add/rmv person
    $('#btn-travel-add-p, #btn-travel-remove-p').on('click', function(e) {
        e.preventDefault();
        var n = $(this).data('pnum');
        $('#frm-so-calc input[name=pers]').val(n);
        tSite.formState($('form.form-stateful:first'), 'save');
        $('#frm-so-calc').submit();
        return false;
    });

    /*
    $('#btn-travel-add-p').on('click', function() {
        var n = $('#div-travel-p-' + tSite.travel.pmax).html();
        var rx = new RegExp('="pers\-([a-z]+)\-' + tSite.travel.pmax + '"', 'gi');
        n = n
                .replace(rx, '="pers-$1-' + (tSite.travel.pmax+1) + '"')
                .replace(/Осигурано лице #[0-9]+/, 'Осигурано лице #' + (tSite.travel.pmax+1));
        $('#div-travel-p-' + tSite.travel.pmax).parent().append( $('<div id="div-travel-p-' + (tSite.travel.pmax+1) + '">' + n + '</div>') );
        tSite.travel.pmax++;
        $('#btn-travel-remove-p').show();
    });
    // remove person
    $('#btn-travel-remove-p').on('click', function() {
        $('#div-travel-p-' + tSite.travel.pmax).remove();
        tSite.travel.pmax--;
        if ( tSite.travel.pmax == 5 ) {
            $(this).hide();
        }
    });
    */



    // - pers details copy
    $.map( ['pers-lname', 'pers-street', 'pers-postno', 'pers-city' ], function(val, i) {
        $('input[name^="' + val + '-"]').on('change', function() {
            var t = this;
            $('input[name^="' + val + '-"]').each(function() {
                if ( this !== t && this.value === '' ) {
                    this.value = t.value;
                    $(this.form).formValidation('revalidateField', this.name);
                }
            });
        });
    });
    // - person travel age...
    $('.so-travel input[name^="pers-bdate-"]').on('blur', function() {
        //lbl-pers-bdate-
        var m = new moment( $(this).val(), tSite.DATE_FORMAT );
        if ( m.isValid() ) {
            $('#lbl-' + this.name).html('Возраст за време на патувањето: <strong>' + tSite.travel.maxDay.diff(m, 'years', false) + 'г.</strong>');
        } else {
            $('#lbl-' + this.name).html('');
        }
    });
    // - person EMBG.>bdate
    $('input[name^="pers-embg-"]').on('blur', function() {
        var t$ = $(this);
        var f$ = $('input[name="' + t$.attr('name').replace(/pers\-embg\-/, "pers-bdate-") + '"]');
        var v = t$.val();
        if ( f$.length > 0 && f$.val().length < 1 && v.length > 6) {
            var m = moment( v.slice(0,2)+'.'+v.slice(2,4)+'.'+(v.slice(4,5)==='9'?'1':'2')+v.slice(4,7), tSite.DATE_FORMAT);
            if ( m.isValid() && m.isBefore( tSite.now ) ) f$.val( m.format(tSite.DATE_FORMAT) );
        }
    });
    // - home package
    $('.so-home-package input[type=radio]').on('click', function() {
      var v = parseInt( $(this).data('val'), 10 );
      if ( !isNaN(v) ) {
        $('#insu-sum').html('Домаќинско осигурување <span>' + tSite.formatNumber(v, 2) + ' ден</span>');
        $('#calc-sum').html('Домаќинско осигурување <span>' + tSite.formatNumber(v, 2) + ' ден</span>');
      }
    });

    // - enable btn
    $('#frm-so-calc select, #frm-so-calc input').on('change keypress', function() {
        $('#insu-sum').fadeTo('def', 0);
        $('#btn-recalc').fadeTo('def', 1);
        return true;
    });
    // - enable btn, date validate / chain update
    $('#frm-so-calc .date-future').on('dp.change', function() {
        var i$ = $(this).find('input');

        var d1 = moment( $('#travel-date-s').val(), tSite.DATE_FORMAT);
        var d2 = moment( $('#travel-date-e').val(), tSite.DATE_FORMAT);

        if ( !d1.isValid() || d1.isBefore(tSite.now) ) {
            d1 = tSite.now.clone().add(1, 'days');
            $('#travel-date-s').val( d1.format(tSite.DATE_FORMAT) );
        }
        if ( !d2.isValid() ) {
            d2 = tSite.now.clone().add(tSite.post['days']-1, 'days');
            $('#travel-date-e').val( d2.format(tSite.DATE_FORMAT) );
        }

        if ( i$.attr('id') === 'travel-date-s' ) {
            $('#travel-date-e').val( d1.add(tSite.post['days']-1,'days').format(tSite.DATE_FORMAT) );
        } else if ( i$.attr('id') === 'travel-date-e' ) {
            var df = d2.diff(d1, 'days', false)+1;
            if ( isNaN(df) ) df = 0;
            if ( df > 90 ) {
                df = 90;
                tSite.post['days'] = df;
                i$.val( d1.add(tSite.post['days']-1,'days').format(tSite.DATE_FORMAT) );
            } else if ( df > 0 ) {
                tSite.post['days'] = df;
            } else {
                i$.val( d1.add(tSite.post['days']-1,'days').format(tSite.DATE_FORMAT) );
            }
        }

        $('#insu-sum').fadeTo('def', 0);
        $('#btn-recalc').fadeTo('def', 1);
        return true;
    });



    // Home

    // - init state
    $('.so-home form#frm-so-calc #home-type').val(tSite.post['type']);
    $('.so-home form#frm-so-calc #home-build').val(tSite.post['build']);
    $('.so-home form#frm-so-calc #home-squares').val(tSite.post['squares']);

    // - helper
    $('a#a-home-compare').on('click', function(e) {
      e.preventDefault();
      tSite.frmWait( $('#home-coverage').html(), 'Usporedba paketa' );
      return false;
    });


    // Form validations

    // - travel
    $('.so-travel form#frm-so-calc').formValidation({
        framework: 'bootstrap',
        locale: tSite.lang['l-code2'],
        // excluded: ':disabled',
        live: 'submitted',
        _last: false
    }).on('success.form.fv', function(e) {
        e.preventDefault();
        var fv = $(e.target).data('formValidation');

        $('#btn-recalc').fadeTo(1, 0);
        fv.defaultSubmit();
    });

    $('.so-travel form#frm-buy, .so-home form#frm-buy').formValidation({
        framework: 'bootstrap',
        locale: tSite.lang['l-code2'],
        excluded: ':disabled',
        trigger: 'blur',
        _last: false
    });
    $('.so-travel form#frm-epay, .so-home form#frm-epay').formValidation({
        framework: 'bootstrap',
        locale: tSite.lang['l-code2'],
        _last: false
    });

    $('#btn-buy').on('click', function(e) {
        e.preventDefault();

        var btn = $(this);

        var fv = $('form#frm-buy').data('formValidation');
        fv.resetForm(false); // valid date cleared is not detected
        fv.validate();

        // DEBUG: console.log( 'Valjid?', fv.isValid(), fv.getInvalidFields().first() );
        if ( !fv.isValid() ) {
            fv.getInvalidFields().first().focus();

        } else {

            // additional validations
            // - contract person of legal age
            if (
                    (  $('#cb-contract').is(':checked') && tSite.now.diff( moment( $('input[name="pers-bdate-1"]').val(), tSite.DATE_FORMAT ), 'years', false) < 18 )
                 || ( !$('#cb-contract').is(':checked') && tSite.now.diff( moment( $('input[name="pers-bdate-u"]').val(), tSite.DATE_FORMAT ), 'years', false) < 18 )
            ) {
                $('#calc-sum').html('<div class="alert alert-danger">лице мора да бидат полнолетни</div>');
                return false;
            }
            $('#calc-sum').html('');


            // let's go...
            btn.prop('disabled', true);
            tSite.frmWait(null);

            setTimeout( function() {
                // recalc
                $.ajax({
                    url: './',
                    method: 'POST',
                    type: 'json',
                    data: 'cmd=calc&' + $('#frm-so-calc').serialize() + '&' + $('#frm-buy').serialize()
                }).always(function(o, status, rq) {

                    tSite.frmWait('hide');
                    btn.prop('disabled', false);

                    //DEBUG:
                    console.log($('#frm-so-calc').serialize() + '&' + $('#frm-buy').serialize());
                    console.log(status);
                    console.log(o);

                    if ( status === 'success' ) {
                        if ( o && o.ok && o.sum > 0 ) {
                            $("#insu-sum").fadeTo(1, 0);

                            var tip = 'travel';
                            if ( o.tip ) { tip = o.tip; }

                            $('#' + tip + '-section-1').slideUp(function() {
                                $('#' + tip + '-section-2 #div-info').html(o.cargo);
                                $('#' + tip + '-section-2').slideDown();
                                $('#btn-back').on('click', function(e) {
                                    $('#' + tip + '-section-2').slideUp(function() {
                                        $('#' + tip + '-section-1').slideDown();
                                    });
                                    e.preventDefault();
                                    return false;
                                });

                                $('#btn-epay').prop('disabled',false).off('click').on('click', function(e) {
                                    e.preventDefault();
                                    var btn = $(this);
                                    var fv = $('form#frm-epay').data('formValidation');
                                    fv.resetForm(false); // valid date cleared is not detected
                                    fv.validate();
                                    if ( !fv.isValid() ) {
                                        fv.getInvalidFields().first().focus();
                                    } else {
                                        tSite.frmWait(null);
                                        btn.prop('disabled', true);
                                        $.ajax({
                                            url: tSite.webshop.authURL,
                                            method: 'POST',
                                            type: 'json',
                                            data: $('#frm-epay').serialize()
                                        }).always(function(o, status, rq) {
                                            //DEBUG: console.log(status, o);
                                            if ( status === 'success' ) {
                                                if ( o && o.ok ) {
                                                    fv.resetForm(false);
                                                    $('#frm-epay').attr('action', './?cmd=pay');
                                                    $('#frm-epay input[name="pay"]').val(o.pp.loc);
                                                    $('#frm-epay input[name="pay-id"]').val(o.pp.paymentID);
                                                    //$('#frm-epay #btn-epay-sub').click();
                                                    document.getElementById('frm-epay').submit(); // IE8+

                                                } else {
                                                    tSite.frmWait('hide');
                                                    tSite.frmWait('<p>' + o.msg + '</p>');
                                                }
                                            } else {
                                                tSite.frmWait('hide');
                                                tSite.frmWait('<p>' + tSite.lang['comm-err'] + '</p<p>' + status + ' ' + o.statusText + '</p>');
                                                btn.prop('disabled', true);
                                            }
                                        });

                                    }
                                    return false;
                                });

                            });

                        } else {
                            $('#calc-sum').html('<div class="alert alert-danger">' + o.cargo + '</div>');
                        }
                    } else {
                        $('#calc-sum').html('<div class="alert alert-danger">' + tSite.lang['comm-err'] + '</div>');
                    }
                });

            }, 1000);

        }

        return false;

    });


    // - travel options
    $('input[data-fields]').on('click', function() {
      var t$ = $(this);
      var e$ = $('[name^="' + t$.data('fields') + '-"]');
      if ( t$.is(':checked') ) {
        e$.prop('disabled', false).removeClass('invisible');
        $('div#' + t$.data('fields') + '-segment').slideDown('fast');
      } else {
        e$.prop('disabled', true).addClass('invisible');
        $('div#' + t$.data('fields') + '-segment').slideUp('fast');
      }
    });
    // - travel pers
    $('input[data-sel-num]').on('change', function() {
      var i = $(this).data('selNum');
      $('option[data-sel-num="' + i + '"]').text( 
          $('input[name="pers-lname-'+i+'"]').val() + ', ' 
          + $('input[name="pers-fname-'+i+'"]').val() 
      );
    });


    // Sum expose
    var fixTimeout = null;
    var fixPosition = function() {
        clearTimeout(fixTimeout);
        var t$ = $("#insu-sum");
        if ( t$.css('position') === 'absolute' ) {
            var wt = tSite.w$.scrollTop();
            var dt = t$.position().top;
            var or = $('#frm-so-calc').position().top + $('#frm-so-calc').height() + 40;
            if ( (wt-80) > or ) {
                t$.stop().animate({'left':$('#frm-so-calc').position().left + 'px', 'top':(wt-80)+'px'}, 500, 'easeOutCubic');
            } else {
                t$.stop().animate({'left':$('#frm-so-calc').position().left + 'px', 'top':or + 'px'}, 500, 'easeOutCubic');
            }
        } else {
            t$.stop().css({'top':'initial', 'left':0});
        }
    };
    $("#insu-sum")
        .html( $('#calc-sum').html() )
        .delay(250)
        .slideDown('fast', function() {
            var t$ = $("#insu-sum");
            tSite.w$.scroll(function() {
                clearTimeout(fixTimeout);
                fixTimeout = setInterval( fixPosition, 250 );
            });
            tSite.w$.resize(function() {
                clearTimeout(fixTimeout);
                fixTimeout = setInterval( fixPosition, 250 );
            });
            fixPosition();
        });




});