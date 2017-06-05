(function($) {
    /**
     * Macedonian /work in progress
     */
    FormValidation.I18n = $.extend(true, FormValidation.I18n, {
        'mk_MK': {
            base64: {
                'default': 'Внесете валиден број b64'
            },
            between: {
                'default': 'Внесете валиден број %s н %s',
                notInclusive: 'Внесете валиден број %s н %s'
            },
            bic: {
                'default': 'Внесете валиден број BIC'
            },
            callback: {
                'default': 'внесете валидна вредност'
            },
            choice: {
                'default': 'внесете валидна вредност',
                less: 'Odaberite najmanje %s opcija',
                more: 'Odaberite najviše % opcija',
                between: 'Odaberite %s do %s opcija'
            },
            color: {
                'default': 'внесете валидна вредност'
            },
            creditCard: {
                'default': 'Внесете валиден број'
            },
            cusip: {
                'default': 'Внесете валиден број CUSIP'
            },
            cvv: {
                'default': 'Внесете валиден број CVV'
            },
            date: {
                'default': 'Внесете валиден датум',
                min: 'Внесете валиден датум по %s',
                max: 'Внесете валиден датум пред %s',
                range: 'Внесете валиден датум %s - %s'
            },
            different: {
                'default': 'Внесете друга вредност'
            },
            digits: {
                'default': 'Внесете валиден број'
            },
            ean: {
                'default': 'Внесете валиден број EAN'
            },
            ein: {
                'default': 'Внесете валиден број EIN'
            },
            emailAddress: {
                'default': 'Внесете редовно е-маил адреса'
            },
            file: {
                'default': 'Odaberite datoteku'
            },
            greaterThan: {
                'default': 'Внесете поголема или еднаква на %s',
                notInclusive: 'Внесете поголема од %s'
            },
            grid: {
                'default': 'Внесете валиден број GRId'
            },
            hex: {
                'default': 'Внесете валиден број HEX'
            },
            iban: {
                'default': 'Внесете валиден број IBAN',
                country: 'Unesite regularni IBAN broj u %s',
                countries: {
                    HR: 'Hrvatsku'
                }
            },
            id: {
                'default': 'Unesite regularni OIB',
                country: 'Unesite regularni OIB za %s',
                countries: {
                    BA: 'BiH',
                    HR: 'Hrvatsku'
                }
            },
            identical: {
                'default': 'Unesite istu vrijednost'
            },
            imei: {
                'default': 'Внесете валиден број IMEI'
            },
            imo: {
                'default': 'Внесете валиден број IMO'
            },
            integer: {
                'default': 'Внесете валиден број'
            },
            ip: {
                'default': 'Unesite regularnu IP adresu',
                ipv4: 'Unesite regularnu IPv4 adresu',
                ipv6: 'Unesite regularnu IPv6 adresu'
            },
            isbn: {
                'default': 'Внесете валиден број ISBN'
            },
            isin: {
                'default': 'Внесете валиден број ISIN'
            },
            ismn: {
                'default': 'Внесете валиден број ISMN'
            },
            issn: {
                'default': 'Внесете валиден број ISSN'
            },
            lessThan: {
                'default': 'Внесете помала или еднаква на %s',
                notInclusive: 'Внесете помала од %s'
            },
            mac: {
                'default': 'Unesite regularnu MAC adresu'
            },
            meid: {
                'default': 'Внесете валиден број MEID'
            },
            notEmpty: {
                'default': 'Ве молам внесете податок'
            },
            numeric: {
                'default': 'Внесете валиден број'
            },
            phone: {
                'default': 'Unesite regularni telefonski broj ',
                country: 'Unesite regularni telefonski broj za %s',
                countries: {
                    HR: 'Hrvatsku'
                }
            },
            regexp: {
                'default': 'Unesite vrijednost prema uzorku'
            },
            remote: {
                'default': 'Unesite regularnu vrijednost'
            },
            rtn: {
                'default': 'Внесете валиден број RTN'
            },
            sedol: {
                'default': 'Внесете валиден број SEDOL'
            },
            siren: {
                'default': 'Внесете валиден број SIREN'
            },
            siret: {
                'default': 'Внесете валиден број SIRET'
            },
            step: {
                'default': 'Внесете валиден број u koraku od %s'
            },
            stringCase: {
                'default': 'Unesite samo mala slova',
                upper: 'unesite samo velika slova'
            },
            stringLength: {
                'default': 'Unesite vrijednost prave dužine',
                less: 'Unesite manje od %s znakova',
                more: 'unesite više od %s znakova',
                between: 'Unesite između %s i %s znakova'
            },
            uri: {
                'default': 'Unesite regularnu adresu URI'
            },
            uuid: {
                'default': 'Внесете валиден број UUID',
                version: 'Unesite regularni UUID broj verzije %s'
            },
            vat: {
                'default': 'Внесете валиден број PDV',
                country: 'Внесете валиден број PDV za %s',
                countries: {
                    AT: 'Austriju',
                    HR: 'Hrvatsku',
                    SI: 'Sloveniju'
                }
            },
            vin: {
                'default': 'Внесете валиден број VIN'
            },
            zipCode: {
                'default': 'Unesite regularni poštanski broj',
                country: 'Unesite regularni poštanski broj za %s',
                countries: {
                    HR: 'Hrvatsku'
                }
            }
        }
    });
}(jQuery));
